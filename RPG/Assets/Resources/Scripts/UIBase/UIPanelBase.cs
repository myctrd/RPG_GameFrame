using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBase : UIBase
{

    public override void Close()
    {
        base.Close();
        UIManager.m_instance.DestroyPanel(gameObject.name);
        if (GameSceneManager.m_instance != null)
        {
            GameSceneManager.m_instance.SetInteraction(true);
        }
    }

    public override void Hide()
    {
        base.Hide();
        UIManager.m_instance.HidePanel(gameObject.name);
    }
}
