using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour {

    public static GameEventManager m_instance;

    void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if (m_instance == null)
        {
            m_instance = this;
        }
    }

    public void ActivateEvent(string eventID)  //触发事件
    {
        Dictionary<string, object> data = CSCallLua.m_instance.GetDBData("event", eventID);
        string[] eventTypes = ((string)data["TYPE"]).Split(new char[1] { ';' });
        string[] events = ((string)data["EVENTS"]).Split(new char[1] { ';' });

        if (eventTypes.Length > 0)
        {
            for (int i = 0; i < eventTypes.Length; i++)
            {
                ExcuteEvent(int.Parse(eventTypes[i]), events[i], data);
            }
        }

    }

    void ExcuteEvent(int m_eventType, string m_event, Dictionary<string, object> data)  //执行事件
    {
        Dictionary<string, object> p = new Dictionary<string, object>();
        string[] info;
        switch (m_eventType)
        {
            case 0:  //弹出对话
                info = m_event.Split(new char[1] { ',' });
                p.Clear();
                p.Add("name", info[0]);
                p.Add("txt", info[1]);
                EventManager.Broadcast("Dialog.CommonDialog", p);
                break;
            case 1:  //直接给奖励
                info = m_event.Split(new char[1] { ',' });
                if (info[0] == "equip")
                {
                    p.Clear();
                    p.Add("id", info[1]);
                    EventManager.Broadcast("Common.GetEquip", p);
                }
                if (info[0] == "item")
                {
                    p.Clear();
                    p.Add("id", info[1]);
                    p.Add("count", info[2]);
                    EventManager.Broadcast("Common.GetItem", p);
                }
                break;
            case 2:  //地图传送
                info = m_event.Split(new char[1] { ',' });
                int mapID = int.Parse(info[0]);
                PlayerPrefs.SetInt("RPGGame_CurrentMap", mapID);
                PlayerPrefs.SetInt("RPGGame_CurrentLine", int.Parse(info[1]));
                PlayerPrefs.SetInt("RPGGame_CurrentCol", int.Parse(info[2]));
                GameManager.m_instance.LoadMap(mapID);
                break;
            case 3:  //解锁故事
                if (PlayerPrefs.HasKey(m_event) == false)
                {
                    PlayerPrefs.SetInt(m_event, 1);
                    p.Clear();
                    p.Add("txt", "StoryUpdated");
                    EventManager.Broadcast("Common.FloatingMsg", p);
                }
                break;
            case 4:  //显示对话
                info = m_event.Split(new char[1] { ',' });
                p.Clear();
                p.Add("name", info[0]);
                p.Add("txt", info[1]);
                p.Add("options", info[2]);
                p.Add("events", info[3]);
                EventManager.Broadcast("Dialog.OptionsDialog", p);
                break;
            case 5:  //打开panel
                info = m_event.Split(new char[1] { ',' });
                LuaScriptManager.m_instance.OpenUIPanel(info[0]);
                if (info.Length > 1)
                {
                    p.Clear();
                    p.Add("data", info[2]);
                    EventManager.Broadcast("UI." + info[1], p);
                }
                break;
            case 6:  //获得金钱
                GameManager.m_instance.AddGold(int.Parse(m_event));
                break;
            case 7:  //显示传闻
                Dictionary<string, object> notes = CSCallLua.m_instance.GetDBData("notes", m_event);
                p.Clear();
                p.Add("name", "Notes");
                p.Add("txt", (string)notes["NAME"]);
                EventManager.Broadcast("Dialog.CommonDialog", p);
                if (PlayerPrefs.HasKey("RPGGame_Notes_" + m_event) == false)
                {
                    PlayerPrefs.SetInt("RPGGame_Notes_" + m_event, 1);
                    DataManager.m_instance.AddNotes((string)notes["ID"], (string)notes["TYPE"]);
                }

                break;
            case 8:  //打开背包
                LuaScriptManager.m_instance.OpenUIPanel("UIPanelBag");
                break;
            case 9:  //战斗
                p.Clear();
                p.Add("enemyID", m_event);
                EventManager.Broadcast("Battle.StartBattle", p);
                break;
            case 10:  //解锁角色
                PlayerPrefs.SetInt("RPGGame_Role_" + m_event, 1);
                GameManager.m_instance.GetNewRole(m_event);
                p.Clear();
                p.Add("txt", m_event + "_JoinTeam");
                EventManager.Broadcast("Common.FloatingMsg", p);
                break;
            default:
                break;
        }
    }
}
