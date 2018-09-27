using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    public static BattleManager m_instance;

    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
    }

    private RoleEnemy enemy;
    private RolePlayer player;

    public RoleEnemy GetEnemyData()
    {
        return enemy;
    }

    public RolePlayer GetPlayerData()
    {
        return player;
    }

    // Use this for initialization
    public void StartBattle (int id)
    {
        player = GameManager.m_instance.GetPlayerData();
        enemy = GameManager.m_instance.GetEnemyData(id);
        EventManager.Broadcast("Battle.UpdateBattleInfo");
    }

    public void EndBattle()
    {
        player = null;
        enemy = null;
        GameManager.m_instance.RemoveEnemyData();
    }

    public void RoleAttackRole(RoleBase a, RoleBase b)  //a攻击b
    {
        b.RoleBeAttacked((float)a.atk, a.cr, a.cv);
        EventManager.Broadcast("Battle.UpdateBattleInfo");
    }
	
}
