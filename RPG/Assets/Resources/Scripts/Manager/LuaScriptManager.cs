using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;

public class LuaScriptManager : MonoBehaviour {

    public static LuaScriptManager m_instance;

    public LuaEnv luaEnv;

    // Use this for initialization
    void Awake () {
		if(m_instance == null)
        {
            m_instance = this;
        }
        luaEnv = new LuaEnv();
        EventManager.InitLuaEnv(luaEnv);
        luaEnv.DoString("require 'main'");
        luaEnv.DoString("require 'global'");
    }

    LuaFunction fun = null;

    void Start()
    {
        InitUI();
    }

    void InitUI()
    {
        fun = luaEnv.Global.Get<LuaFunction>("InitUI");
        fun.Call();
    }

    public void InitLuaLanguage(string language)
    {
        fun = luaEnv.Global.Get<LuaFunction>("InitLanguage");
        fun.Call(language);
    }

    public void OpenUIPanel(string panel)
    {
        fun = luaEnv.Global.Get<LuaFunction>("OpenUIPanel");
        fun.Call(panel);
    }

    private void OnDestroy()
    {
        fun = null;
        EventManager.onDestroy();
        CSCallLua.m_instance.onDestroy();
        if (luaEnv != null)
        {
            luaEnv.Dispose();
            luaEnv = null;
        }
    }
}

[LuaCallCSharp]
public class EventManager
{
    public delegate void EvtCallback(string eventName, EvtData data);

    //事件回调数据
    public class EvtData
    {
        private LuaTable luadata = null;
        private Dictionary<string, object> data = null;

        public EvtData(LuaTable data)
        {
            this.luadata = data;
        }

        public EvtData(Dictionary<string, object> data)
        {
            this.data = data;
        }

        public LuaTable getLuaData()
        {
            if (luadata != null)
            {
                return luadata;
            }

            if (data != null)
            {
                luadata = CSCallLua.m_instance.DictionaryToLuaTable(data, m_luaEnv);
            }
            return luadata;
        }

        public Dictionary<string, object> getData()
        {
            if (data != null)
            {
                return data;
            }
            if (luadata != null)
            {
                /*data = new Dictionary<string, object>();
                luadata.ForEach<string, object>((name, value) =>
                {
                    data.Add(name, value);
                });
                return data;*/
                return luadata.Cast<Dictionary<string, object>>();
            }

            return null;
        }

    }

    //注册回调的表
    private static Dictionary<string, HashSet<EvtCallback>> m_Subscribed = new Dictionary<string, HashSet<EvtCallback>>();
    private static int m_InCallLoops = 0;
    private static List<KeyValuePair<string, EvtCallback>> m_ItemsToRemove = new List<KeyValuePair<string, EvtCallback>>();

    [BlackList]
    public static void AddListener(string eventName, EvtCallback fun)
    {
        if (!m_Subscribed.ContainsKey(eventName))
            m_Subscribed.Add(eventName, new HashSet<EvtCallback>());

        m_Subscribed[eventName].Add(fun);
        //Debug.Log("[Event] Subscribe:" + eventName + ", " + fun.ToString() );
    }
    [BlackList]
    public static void RemoveListener(string eventName, EvtCallback fun)
    {
        if (!m_Subscribed.ContainsKey(eventName))
            return;

        if (m_InCallLoops > 0)  //如果在回调中调用Unsub
            m_ItemsToRemove.Add(new KeyValuePair<string, EvtCallback>(eventName, fun));
        else
        {
            m_Subscribed[eventName].Remove(fun);
            if (m_Subscribed[eventName].Count == 0)
                m_Subscribed.Remove(eventName);
            //Debug.Log("[Event] Unsubscribe OK:" + eventName + ", " + fun.ToString());
        }
    }

    private static Dictionary<string, object> m_paramList = new Dictionary<string, object>();
    [BlackList]
    public static void Broadcast(string eventName, params KeyValuePair<string, object>[] data)
    {
        //var args = new List<object>();
        m_paramList.Clear();
        for (int i = 0; i < data.Length; i++)
        {
            m_paramList.Add(data[i].Key, data[i].Value);
        }

        EventManager.Broadcast(eventName, m_paramList);
        m_paramList.Clear();
    }
    [BlackList]
    public static void Broadcast(string eventName, Dictionary<string, object> list = null)
    {
        Broadcast(eventName, list != null ? new EvtData(list) : null);
    }
    [BlackList]
    public static void Broadcast(string eventName, EvtData data)
    {
        if (m_Subscribed.ContainsKey(eventName))
        {
            //Debug.Log("[Event] Fired: " + eventName);
            m_InCallLoops = m_InCallLoops + 1;
            try
            {
                foreach (var v in m_Subscribed[eventName])
                {
                    v(eventName, data);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message + "\n" + e.StackTrace);
                throw;
            }
            finally
            {
                m_InCallLoops = m_InCallLoops - 1;
                if (m_InCallLoops <= 0)
                {
                    m_InCallLoops = 0;
                    foreach (var item in m_ItemsToRemove)
                    {
                        if (m_Subscribed.ContainsKey(item.Key))
                        {
                            //因为有嵌套，所以remove时可能是子嵌套登记的event，所以要子嵌套告诉你eventname
                            if (m_Subscribed[item.Key].Contains(item.Value))
                                m_Subscribed[item.Key].Remove(item.Value);
                            if (m_Subscribed[item.Key].Count == 0)
                                m_Subscribed.Remove(item.Key);
                            //Debug.Log("[Event] Unsubscribe OK:" + item.Key + ", " + item.ToString());
                        }

                    }
                    m_ItemsToRemove.Clear();
                }
            }
        }
        else
        {
            //Debug.Log("[Event] Fired, but no reicver: " + eventName);
        }
    }

    //==========================================================================================================================================
    //只支持单个lua vm
    private static Dictionary<string, LuaFunction> m_LuaSubscribed = new Dictionary<string, LuaFunction>();
    private static int m_IsInLuaCallLoop = 0;
    private static LuaEnv m_luaEnv = null;
    private static List<string> m_LuaItemsToRemove = new List<string>();

    public static void InitLuaEnv(LuaEnv l)
    {
        m_luaEnv = l;
    }

    public static void onDestroy()
    {
        m_LuaSubscribed.Clear();
    }
    public static void AddLuaListener(string eventName, LuaFunction fun)
    {
        if (!m_LuaSubscribed.ContainsKey(eventName))
        {
            m_LuaSubscribed.Add(eventName, fun);
            AddListener(eventName, OnLuaBroadcast);
        }
        else
        {
            Debug.LogError("this string is addLuaListener ,please change to other!");
        }




    }
    public static int RemoveLuaListener(string eventName)
    {
        if (!m_LuaSubscribed.ContainsKey(eventName))
            return 0;



        if (m_IsInLuaCallLoop > 0)//如果是在callback中调用Unsub
        {
            m_LuaItemsToRemove.Add(eventName);
        }
        else
        {
            m_LuaSubscribed.Remove(eventName);
            RemoveListener(eventName, OnLuaBroadcast);

        }


        return 0;
    }

    private static void OnLuaBroadcast(string eventName, EvtData res)
    {
        if (m_LuaSubscribed.ContainsKey(eventName))
        {
            var eventStats = m_LuaSubscribed[eventName];
            LuaTable luaTabe = res != null ? res.getLuaData() : null;

            m_IsInLuaCallLoop = m_IsInLuaCallLoop + 1;
            try
            {
                eventStats.Call(eventName, luaTabe);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message + "\n" + e.StackTrace);
                throw;
            }
            finally
            {
                m_IsInLuaCallLoop = m_IsInLuaCallLoop - 1;
                if (m_IsInLuaCallLoop <= 0)
                {
                    m_IsInLuaCallLoop = 0;
                    foreach (var item in m_LuaItemsToRemove)
                    {
                        m_LuaSubscribed.Remove(item);
                        RemoveListener(item, OnLuaBroadcast);

                    }
                    m_LuaItemsToRemove.Clear();
                }
            }
        }
    }

    public static void LuaBroadcast(string eventName, LuaTable luadata)
    {
        EventManager.Broadcast(eventName, new EvtData(luadata));
    }
}
