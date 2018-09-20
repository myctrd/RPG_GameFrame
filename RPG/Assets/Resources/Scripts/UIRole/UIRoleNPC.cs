using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoleNPC : UIRoleBase
{

    public override void SetUIRole(int id, int line, int col, float pos_x, float pos_y)
    {
        base.SetUIRole(id, line, col, pos_x, pos_y);
        Dictionary<string, object> data = CSCallLua.m_instance.GetDBData("npc", id.ToString());
        img.sprite = UIResourceLoader.m_instance.Load<Sprite>("Textures/Role/UIRole/Role_" + (string)data["SPRITEID"] + ".png");
    }
    
}
