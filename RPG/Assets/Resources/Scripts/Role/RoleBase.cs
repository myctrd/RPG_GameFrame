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

}

public enum RoleType
{
    player = 1,
    npc = 2,
    enemy = 3,
}