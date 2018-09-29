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

    private int turnIndex;

    // Use this for initialization
    public void StartBattle (int id)
    {
        //player = GameManager.m_instance.GetPlayerData();
        enemy = GameManager.m_instance.GetEnemyData(id);
        EventManager.Broadcast("Battle.UpdateBattleInfo");
        turnIndex = 0;
        SwitchTurn();
    }

    public void SwitchTurn()
    {
        Dictionary<string, object> p = new Dictionary<string, object>();
        if (turnIndex == 0)  //玩家回合
        {
            p.Add("txt", "YourTurn");
            turnIndex = 1;
            EventManager.Broadcast("Battle.YourTurn");
        }
        else  //敌人回合
        {
            p.Add("txt", "EnemyTurn");
            turnIndex = 0;
            StartCoroutine("EnemyAction");
        }
        EventManager.Broadcast("Common.FloatingMsg", p);
    }

    IEnumerator EnemyAction()
    {
        yield return new WaitForSeconds(1);
        if (enemy != null)
        {
            RoleAttackRole(enemy, player);
            yield return new WaitForSeconds(1);
            if (enemy != null)
            {
                SwitchTurn();
            }
        }
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
