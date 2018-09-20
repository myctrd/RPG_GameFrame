using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoleEnemy : RoleBase
{ 
    public void SetRoleEnemy(int id)
    {
        Dictionary<string, object> data = CSCallLua.m_instance.GetDBData("role", id.ToString());
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
    }
}
