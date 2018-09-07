using System.Collections;
using System.Collections.Generic;
using XLua;
using UnityEngine;

[XLua.LuaCallCSharp]

public class LuaCallCSUtils {
    
    public static LuaCallCSUtils m_instance;

    public LuaCallCSUtils()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
    }
    
    public static void PrintTest()
    {
        Debug.Log("PrintTest");
    }

    public static int[] GetRoleAvailable()
    {
        return GameManager.m_instance.GetRoleAvailable();
    }

    public static void SetRolePlayer(int role)
    {
        GameManager.m_instance.SetRolePlayer(role);
    }

    public static RoleBase GetPlayerData()
    {
        return GameManager.m_instance.GetPlayerData();
    }
}
