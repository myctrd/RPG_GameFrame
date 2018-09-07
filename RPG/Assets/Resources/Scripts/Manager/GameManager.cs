using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager m_instance;

    private int [] roleAvailable = new int[4];

    void Awake()
	{
		if(m_instance == null)
		{
			m_instance = this;
		}
	}

    void Start()
    {
        SetRoleAvailable(0, 1);
        SetRoleAvailable(1, 0);
        SetRoleAvailable(2, 0);
        SetRoleAvailable(3, 0);
    }

    public void SetRoleAvailable(int i, int v)
    {
        roleAvailable[i] = v;
    }

    public int[] GetRoleAvailable()
    {
        return roleAvailable;
    }

    private RolePlayer m_Player;

    public RolePlayer GetPlayerData()
    {
        return m_Player;
    }

    public void SetRolePlayer(int role)
    {
        if(m_Player == null)
        {
            m_Player = gameObject.AddComponent<RolePlayer>();
        }
        m_Player.SetRolePlayer(role);
    }

    public void SetPlayerEquip(int slot, int id)
    {
        m_Player.SetPlayerEquip(slot, id);
    }

}

