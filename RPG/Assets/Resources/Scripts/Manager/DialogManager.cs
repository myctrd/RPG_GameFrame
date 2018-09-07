using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour {

    public static DialogManager m_instance;

    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
    }

    // Update is called once per frame
    public void ShowDialog (string fileName) {
        GameObject c = UIManager.m_instance.LoadComponent("UIComponentDialog", null, 2).gameObject;
        UIComponentDialog ui = c.GetComponent<UIComponentDialog>();
        ui.InitDialog(fileName);
    }

    void DialogFunc(NotifyEvent evt)
    {
        if (evt.Params["func"] == "LoadScene_1")
        {
            UIManager.m_instance.ShowOrCreatePanel("UIPanelGameDeck");
        }
    }

    void Start()
    {
        NotificationCenter.m_instance.RegisterObserver(NotifyType.DialogFunc, DialogFunc);
    }
}
