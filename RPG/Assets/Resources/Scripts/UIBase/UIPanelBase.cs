using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBase : UIBase
{

    public override void Close()
    {
        base.Close();
        UIManager.m_instance.DestroyPanel(gameObject.name);
    }
}
