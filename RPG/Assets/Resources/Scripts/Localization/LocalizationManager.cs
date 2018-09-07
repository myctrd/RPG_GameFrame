using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour {

    public static LocalizationManager m_instance;

    private string m_Language;

    // Use this for initialization
    void Awake () {
		if(m_instance == null)
        {
            m_instance = this;
        }
    }

    void Start()
    {
        InitLanguage();
    }

    public string GetLanguage()
    {
        return m_Language;
    }

    public void InitLanguage()
    {
        string str_lan = PlayerPrefs.GetString("RPGTEST_LANGUAGE");
        switch(str_lan)
        {
            case "CHINESE":
                m_Language = "CHINESE";
                break;
            case "ENGLISH":
                m_Language = "ENGLISH";
                break;
            default:
                m_Language = "CHINESE";
                break;
        }
        LuaScriptManager.m_instance.InitLuaLanguage(m_Language);
    }

    public string GetStringByID(string id)
    {
        Dictionary<string, object> data = CSCallLua.m_instance.GetDBData("localization", id);
        if(data[m_Language] != null)
        {
            return (string)data[m_Language];
        }
        else
        {
            return "";
        }
    }

    public void SwitchLanguage()
    {
        if(m_Language == "ENGLISH")
        {
            PlayerPrefs.SetString("RPGTEST_LANGUAGE", "CHINESE");
        }
        else if (m_Language == "CHINESE")
        {
            PlayerPrefs.SetString("RPGTEST_LANGUAGE", "ENGLISH");
        }
        InitLanguage();
        EventManager.Broadcast("Language.RefreshText");
    }
}

