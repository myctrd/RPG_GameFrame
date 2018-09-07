using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationText : MonoBehaviour
{

    public string ID;

    // Use this for initialization
    void OnEnable()
    {
        EventManager.AddListener("Language.RefreshText", RefreshText);
    }

    void OnDestroy()
    {
        EventManager.RemoveListener("Language.RefreshText", RefreshText);
    }

    void Start()
    {
        GetString();
    }

    void RefreshText(string eventName, EventManager.EvtData data)
    {
        GetString();
    }

    void GetString()
    {
        Text text = transform.GetComponent<Text>();
        if (text != null)
        {
            text.text = LocalizationManager.m_instance.GetStringByID(ID);
        }
    }

}
