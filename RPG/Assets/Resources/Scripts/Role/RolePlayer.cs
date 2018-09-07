using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RolePlayer : RoleBase
{
    public int[] equip = new int[6];  //武器槽

    private int roleIndex;
	public void SetRolePlayer(int role)
    {
        m_Type = RoleType.player;
        roleIndex = role;
        UpdatePlayerAttr();
    }

    void UpdatePlayerAttr()
    {
        Dictionary<string, object> data = CSCallLua.m_instance.GetDBData("role", roleIndex.ToString());
        m_ID = (string)data["ID"];
        m_Name = (string)data["ROLENAME"];

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

        for (int i = 0; i < 6; i++)
        {
            if (PlayerPrefs.HasKey(m_Name + "Equip_" + (i + 1).ToString()))
            {
                equip[i] = PlayerPrefs.GetInt(m_Name + "Equip_" + (i + 1).ToString());
            }
            else
            {
                equip[i] = 0;
            }
            if (equip[i] != 0)
            {
                AddEquipBuff(equip[i]);
            }
        }
    }

    void AddEquipBuff(int id)
    {
        Dictionary<string, object> data = CSCallLua.m_instance.GetDBData("equip", id.ToString());
        string buffs = ((string)data["BUFF"]);
        string [] buff = buffs.Split(new char[1] { ';'});
        for (int i = 0; i < buff.Length; i++)
        {
            string[] value = buff[i].Split(new char[1] { ',' });
            if(value[0] == "1")
            {
                hp = hp + int.Parse(value[1]);
                maxHp = maxHp + int.Parse(value[1]);
            }
            else if (value[0] == "2")
            {
                mp = mp + int.Parse(value[1]);
                maxMp = maxMp + int.Parse(value[1]);
            }
            else if (value[0] == "3")
            {
                atk = atk + int.Parse(value[1]);
            }
            else if (value[0] == "4")
            {
                ap = ap + int.Parse(value[1]);
            }
            else if (value[0] == "5")
            {
                amr = amr + int.Parse(value[1]);
            }
            else if (value[0] == "6")
            {
                mr = mr + int.Parse(value[1]);
            }
            else if (value[0] == "7")
            {
                cr = cr + float.Parse(value[1]);
            }
            else if (value[0] == "8")
            {
                cv = cv + float.Parse(value[1]);
            }
            //1.血量
            //2.魔量
            //3.攻击
            //4.法强
            //5.护甲
            //6.魔抗
            //7.暴击几率
            //8.暴击值
        }
    }

    public void SetPlayerEquip(int slot, int id)
    {
        equip[slot] = id;
        PlayerPrefs.SetInt(m_Name + "Equip_" + (slot + 1).ToString(), id);
        UpdatePlayerAttr();
    }
}
