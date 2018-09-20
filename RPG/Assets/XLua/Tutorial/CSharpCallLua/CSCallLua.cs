/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;

public class CSCallLua : MonoBehaviour {
    public static CSCallLua m_instance;
    
    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
    }
    
    public class DClass
    {
        public int f1;
        public int f2;
    }
    
    [CSharpCallLua]
    public interface ItfD
    {
        int f1 { get; set; }
        int f2 { get; set; }
        int add(int a, int b);
    }

    [CSharpCallLua]
    public delegate int FDelegate(int a, string b, out DClass c);

    [CSharpCallLua]
    public delegate Action GetE();
    
    // Use this for initialization

    [CSharpCallLua]
    public delegate LuaTable LuaDBFind(string table, object key);

    private static LuaDBFind luaDBFind = null;
    

    public Dictionary<string, object> GetDBData(string table, string key)
    {
        if (luaDBFind == null)
        {
            luaDBFind = LuaScriptManager.m_instance.luaEnv.Global.GetInPath<LuaDBFind>("GetData");
        }
        
        Dictionary<string, object> db = null;
        if (luaDBFind != null)
        {
            LuaTable lt = luaDBFind(table, key);
            if (lt != null)
            {
                db = LuaTableToDictionary(lt);
            }
        }
        else
        {
            Debug.LogError("db null");
        }
        return db;
    }

    public LuaTable DictionaryToLuaTable(Dictionary<string, object> tableData = null, LuaEnv luaEnv = null)
    {
        if (luaEnv == null)
        {
            luaEnv = LuaScriptManager.m_instance.luaEnv;
        }
        LuaTable luatabel = luaEnv.NewTable();
        if (tableData != null)
        {
            foreach (var v in tableData)
            {
                if (v.Value is Dictionary<string, object>)
                {
                    luatabel.Set<string, object>(v.Key, DictionaryToLuaTable(v.Value as Dictionary<string, object>, luaEnv));
                }
                else
                {
                    luatabel.Set<string, object>(v.Key, v.Value);
                }
            }
        }
        return luatabel;
    }

    public static Dictionary<string, object> LuaTableToDictionary(LuaTable luaTb)
    {
        if (luaTb != null)
        {
            return luaTb.Cast<Dictionary<string, object>>(); ;
        }
        else
        {
            return null;
        }
    }
    
    public void onDestroy()
    {
        luaDBFind = null;
    }
}
