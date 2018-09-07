using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelGameDeck : UIPanelBase
{

    void Start()
    {
        InitUI();
    }

    private Transform content, secneRoot;

    void InitUI()
    {
        content = transform.Find("scrollview_item/Viewport/content");
        secneRoot = transform.Find("sceneRoot");
        LoadScene(1);
    }

    void LoadScene(int i)
    {
        if(secneRoot)
        {
           UIManager.m_instance.LoadScene(i, secneRoot);
        }
    }

    public void OnClickQuit()
    {
        Close();
        UIManager.m_instance.ShowOrCreatePanel("UIPanelMenu");
    }

    void GetItem(NotifyEvent evt)
    {
        
    }

    void OnEnable()
    {
        NotificationCenter.m_instance.RegisterObserver(NotifyType.GetItem, GetItem);
    }

    void OnDestroy()
    {
        NotificationCenter.m_instance.RemoveObserver(NotifyType.GetItem, GetItem);
    }
}
