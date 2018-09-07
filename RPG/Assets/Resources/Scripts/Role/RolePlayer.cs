using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolePlayer : RoleBase
{
    public int[] equip = new int[6];  //武器槽
	public void SetRolePlayer(int role)
    {
        m_Type = RoleType.player;

        Dictionary<string, object> data = CSCallLua.m_instance.GetDBData("role", role.ToString());
        m_ID = (string)data["ID"];
        m_Name = (string)data["ROLENAME"];

        for(int i = 0; i < 6; i++)
        {
            if (PlayerPrefs.HasKey(m_Name + "Equip_" + (i + 1).ToString()))
            {
                equip[i] = PlayerPrefs.GetInt(m_Name + "Equip_" + (i + 1).ToString());
            }
            else
            {
                equip[i] = 0;
            }
        }
        

        m_Pro = int.Parse((string)data["PRO"]);
        hp = int.Parse((string)data["BASICHP"]);
        maxHp = int.Parse((string)data["BASICHP"]);
        mp = int.Parse((string)data["BASICMP"]);
        maxMp = int.Parse((string)data["BASICMP"]);
        atk = int.Parse((string)data["BASICAD"]);
        ap = int.Parse((string)data["BASICAP"]); 
        amr = int.Parse((string)data["BASICARMOR"]); 
        mr = int.Parse((string)data["BASICMR"]);
        cr = float.Parse((string)data["BASICCRIRATE"]); 
        cv = float.Parse((string)data["BASICCRIVAL"]); 
    }
}
