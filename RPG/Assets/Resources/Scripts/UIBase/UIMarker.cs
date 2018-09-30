using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMarker : UIBase
{
    public int status = 0;  //用于临时记录状态

    public void SetButtonInteractable(bool state)
    {
        if (transform.GetComponent<Button>() != null)
        {
            var button = transform.GetComponent<Button>();
            button.interactable = state;
        }
    }

    public void RemoveListener()
    {
        if (transform.GetComponent<Button>() != null)
        {
            var button = transform.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
        }
    }

    public void AddListener(UnityAction call) {
        if(transform.GetComponent<Button>() != null)
        {
            var button = transform.GetComponent<Button>();
            button.onClick.AddListener(call);
        }
    }

    public void SetText(string txt)
    {
        if (transform.GetComponent<Text>() != null)
        {
            var text = transform.GetComponent<Text>();
            text.text = txt;
        }
    }

    public void SetTextColor(float r, float g, float b)
    {
        if (transform.GetComponent<Text>() != null)
        {
            var text = transform.GetComponent<Text>();
            text.color = new Color(r, g, b);
        }
    }

    public void SetImageColor(float r, float g, float b)
    {
        if (transform.GetComponent<Image>() != null)
        {
            var image = transform.GetComponent<Image>();
            image.color = new Color(r, g, b);
        }
    }

    public void SetSprite(string sprite)
    {
        if (transform.GetComponent<Image>() != null)
        {
            var image = transform.GetComponent<Image>();
            image.sprite = UIResourceLoader.m_instance.Load<Sprite>("Textures/" + sprite + ".png");
        }
    }

    public void SetFilledValue(float val)
    {
        if (transform.GetComponent<Slider>() != null)
        {
            var slider = transform.GetComponent<Slider>();
            slider.value = val;
        }
    }

    public void SetActive(bool v)
    {
        gameObject.SetActive(v);
    }

    public void ClearChild()
    {
        int childCount = transform.childCount;
        for (int i = childCount; i > 0; i--)
        {
            Destroy(transform.GetChild(i - 1).gameObject);
        }

    }

    public Transform GetTransfrom()
    {
        return transform;
    }

    public void SetGameObjectName(string name)
    {
        gameObject.name = name;
    }

    public string GetGameObjectName()
    {
        return gameObject.name;
    }
}
