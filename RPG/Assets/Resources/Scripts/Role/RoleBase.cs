using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleBase : MonoBehaviour {

    protected RoleType m_Type;
    public string m_ID { get; set; }
    public string m_Name { set; get; }
    public int m_Pro { set; get; }
    public int hp { get; set; }
    public int maxHp { get; set; }
    public int mp { get; set; }
    public int maxMp { get; set; }
    public int atk { get; set; }
    public int ap { get; set; }
    public int amr { get; set; }
    public int mr { get; set; }
    public float cr { get; set; }
    public float cv { get; set; }

    public virtual void RoleBeAttacked(float damage, float cv, float cr)
    {
        if (Random.Range(0.0f, 1.0f) < cv)
        {
            damage = damage * cr;
            Dictionary <string, object> p = new Dictionary<string, object>();
            p.Add("txt", "Critical!");
            EventManager.Broadcast("Battle.BattleTips", p);
        }
        float cutDamage = (1 - (amr * 0.06f) / (1 + amr * 0.06f)) * damage;
        int realDamage = Random.Range(0.0f, 1.0f) < cutDamage - (int)cutDamage ? (int)cutDamage + 1 : (int)cutDamage;
        //Debug.LogError(realDamage);
        hp = hp - realDamage < 0 ? 0 : hp - realDamage;
    }

}

public enum RoleType
{
    player = 1,
    npc = 2,
    enemy = 3,
}