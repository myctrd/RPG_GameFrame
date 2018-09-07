using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIComponentSceneItem : UIComponentBase
{
    public int itemId;
    private Image icon;
    // Use this for initialization
    void Start () {
        InitUI();
    }

    void InitUI()
    {
        icon = transform.Find("icon").GetComponent<Image>();
        icon.sprite = UIResourceLoader.m_instance.LoadItemSprite(itemId);
    }
    
    public void OnClick()
    {
        NotifyEvent evt = new NotifyEvent(NotifyType.GetItem, null);
        evt.Params.Add("itemId", itemId.ToString());
        NotificationCenter.m_instance.PostNotification(evt);
        Close();
    }
}
