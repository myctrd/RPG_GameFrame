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

    public void ActivateEvent(string eventID)  //触发事件
    {
        Dictionary<string, object> data = CSCallLua.m_instance.GetDBData("event", eventID);
        string[] eventTypes = ((string)data["TYPE"]).Split(new char[1] { ';' });
        string[] events = ((string)data["EVENTS"]).Split(new char[1] { ';' });
        
        if(eventTypes.Length > 0)
        {
            for(int i = 0; i < eventTypes.Length; i ++)
            {
                ExcuteEvent(int.Parse(eventTypes[i]), events[i], data);
            }
        }
        
    }

    void ExcuteEvent(int m_eventType, string m_event, Dictionary<string, object> data)  //执行事件
    {
        Dictionary<string, object> p = new Dictionary<string, object>();
        string[] info;
        switch (m_eventType)
        {
            case 0:  //弹出对话
                info = m_event.Split(new char[1] { ',' });
                if (info[0] == "npc")
                {
                    p.Clear();
                    p.Add("name", info[1]);
                    p.Add("txt", info[2]);
                    EventManager.Broadcast("Dialog.CommonDialog", p);
                }
                break;
            case 1:  //直接给奖励
                info = m_event.Split(new char[1] { ',' });
                if (info[0] == "equip")
                {
                    p.Clear();
                    p.Add("id", info[1]);
                    EventManager.Broadcast("Common.GetEquip", p);
                }
                break;
            case 2:  //地图传送
                info = m_event.Split(new char[1] { ',' });
                int mapID = int.Parse(info[0]);
                PlayerPrefs.SetInt(GameManager.m_instance.GetPlayerData().m_Name + "_CurrentMap", mapID);
                PlayerPrefs.SetInt(GameManager.m_instance.GetPlayerData().m_Name + "_CurrentLine", int.Parse(info[1]));
                PlayerPrefs.SetInt(GameManager.m_instance.GetPlayerData().m_Name + "_CurrentCol", int.Parse(info[2]));
                GameManager.m_instance.LoadMap(mapID);
                break;
            case 3:  //解锁故事
                if (PlayerPrefs.HasKey(m_event) == false)
                {
                    PlayerPrefs.SetInt(m_event, 1);
                    p.Clear();
                    p.Add("txt", "StoryUpdated");
                    EventManager.Broadcast("Common.FloatingMsg", p);
                }
                break;
            default:
                break;
        }
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
        EventManager.Broadcast("Role.UpdateRoleInfo");
    }
}
