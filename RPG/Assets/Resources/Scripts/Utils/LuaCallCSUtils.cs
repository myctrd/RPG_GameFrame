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

    public static void SetPlayerEquip(int slot, int id)
    {
        GameManager.m_instance.SetPlayerEquip(slot, id);
    }

    public static void StartBattle(int id)
    {
        BattleManager.m_instance.StartBattle(id);
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

    public static void LoadData()
    {
        DataManager.m_instance.LoadEquipData();
        DataManager.m_instance.LoadItemData();
    }

    public static void AddEquip(string id, string count)
    {
        DataManager.m_instance.AddEquip(id, count);
    }

    public static void SetInteraction(bool state)
    {
        if(GameSceneManager.m_instance != null)
        {
            GameSceneManager.m_instance.SetInteraction(state);
        }
    }

    public static bool PlayerPrefsHasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
}
