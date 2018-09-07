using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIComponentDialog : UIComponentBase
{
    private Text txt_content;
    private Text[] txt_name = new Text[2];
    private Image[] img_role = new Image[2];
    private int step = 0, maxStep = 0;
    private string[] dialogs;
    // Use this for initialization

    void InitUI()
    {
        txt_content = transform.Find("txt_content").GetComponent<Text>();
        txt_name[0] = transform.Find("img_role_0/img_name/txt_name").GetComponent<Text>();
        txt_name[1] = transform.Find("img_role_1/img_name/txt_name").GetComponent<Text>();
        img_role[0] = transform.Find("img_role_0").GetComponent<Image>();
        img_role[1] = transform.Find("img_role_1").GetComponent<Image>();
    }

    public void InitDialog(string fileName)
    {
        InitUI();
        step = 0;
        dialogs = UIResourceLoader.m_instance.LoadDialog("Dialogs/" + fileName + ".txt");
        maxStep = dialogs.Length;
        NextStep();
    }

    public void NextStep()
    {
        if(step + 1 <= maxStep)
        {
            string[] str = dialogs[step].Split('@');
            img_role[0].gameObject.SetActive(str[0] == "0");
            img_role[1].gameObject.SetActive(str[0] == "1");
            txt_content.text = str[2];
            if (str[0] == "0")
            {
                img_role[0].sprite = UIResourceLoader.m_instance.Load<Sprite>("Textures/Avatar/Role_" + str[1] + ".png");
            }
            else
            {
                img_role[1].sprite = UIResourceLoader.m_instance.Load<Sprite>("Textures/Avatar/Role_" + str[1] + ".png");
            }
            step++;
        }
        else
        {
            string[] str = dialogs[step - 1].Split('@');
            if(str.Length > 3 && str[3] != null)
            {
                NotifyEvent evt = new NotifyEvent(NotifyType.DialogFunc, null);
                evt.Params.Add("func", str[3]);
                NotificationCenter.m_instance.PostNotification(evt);
            }
            Close();
        }
    }
    
}
