using UnityEngine;
using System.Collections.Generic;

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

    public static void ClearArchive()
    {
        PlayerPrefs.DeleteAll();
        DataManager.m_instance.DeleteAll();
        LocalizationManager.m_instance.InitLanguage();
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

    public static int GetPlayerGold()
    {
        return GameManager.m_instance.GetPlayerGold();
    }

    public static void SetPlayerEquip(int slot, int id)
    {
        GameManager.m_instance.SetPlayerEquip(slot, id);
    }

    public static void StartBattle(int id)
    {
        BattleManager.m_instance.StartBattle(id);
    }

    public static void SwitchTurn()
    {
        BattleManager.m_instance.SwitchTurn();
    }

    public static void EndBattle()
    {
        BattleManager.m_instance.EndBattle();
    }

    public static RolePlayer GetBattlePlayerData()
    {
        return BattleManager.m_instance.GetPlayerData();
    }

    public static RoleEnemy GetEnemyData()
    {
        return BattleManager.m_instance.GetEnemyData();
    }

    public static void RoleAttackRole(RoleBase a, RoleBase b)  //a攻击b
    {
        BattleManager.m_instance.RoleAttackRole(a, b);
    }

    public static void LoadGameScene()
    {
        GameManager.m_instance.LoadGameScene();
    }

    public static void UnloadGameScene()
    {
        GameManager.m_instance.LoadScene("Void");
    }

    public static void LoadItemData()
    {
        DataManager.m_instance.LoadEquipData();
        DataManager.m_instance.LoadItemData();
    }

    public static void LoadEventData()
    {
        DataManager.m_instance.LoadNotesData();
    }

    public static void AddEquip(string id, string count)
    {
        DataManager.m_instance.AddEquip(id, count);
    }

    public static void UpdateItem(string id, string count)
    {
        DataManager.m_instance.UpdateItem(id, count);
    }

    public static void SetInteraction(bool state)
    {
        if (GameSceneManager.m_instance != null)
        {
            GameSceneManager.m_instance.SetInteraction(state);
        }
    }

    public static bool PlayerPrefsHasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void PlayerPrefSetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static void ActivateEvent(string eventID)
    {
        GameManager.m_instance.GetPlayerData().ActivateEvent(eventID);
    }

    public static void AddGold(int value)
    {
        GameManager.m_instance.AddGold(value);
    }

    public static void ShowItem(string id)
    {
        if(GameSceneManager.m_instance.GetInteractiveNPC() == 0)
        {
            Dictionary<string, object> p = new Dictionary<string, object>();
            p.Add("txt", "Emm");
            EventManager.Broadcast("Common.FloatingMsg", p);
        }
        else
        {
            int npcID = GameSceneManager.m_instance.GetInteractiveNPC();
            Dictionary<string, object> data = CSCallLua.m_instance.GetDBData("npc", npcID.ToString());
            if((string)data["NEEDITEM"] == id)
            {
                Dictionary<string, object> p = new Dictionary<string, object>();
                p.Add("id", id);
                EventManager.Broadcast("DataManager.ConsumeTaskItem", p);
                PlayerPrefs.SetInt(GameManager.m_instance.GetPlayerData().m_Name + "_Event_" + (string)data["EVENT"], 1);
                GameManager.m_instance.GetPlayerData().ActivateEvent((string)data["REWARDEVENT"]);
            }
            else
            {
                Dictionary<string, object> p = new Dictionary<string, object>();
                p.Add("txt", "Emm");
                EventManager.Broadcast("Common.FloatingMsg", p);
            }
        }
    }
}
