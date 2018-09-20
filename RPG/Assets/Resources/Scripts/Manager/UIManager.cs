using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour {

	public static UIManager m_instance;

	Dictionary<string, UIPanelBase> _acvtive_panels = new Dictionary<string, UIPanelBase>();
    List<Transform> _sort_panels = new List<Transform>();
    Transform root = null;
	
    void Awake()
    {
        if(m_instance == null)
		{
			m_instance = this;
		}
		root = transform.Find("Root");
		if(root)
		{
			int count = root.childCount;
			for (int i = 0; i < count; i++)
			{
				_sort_panels.Add(root.GetChild(i));
			}
		}
    }
	
	public UIPanelBase ShowOrCreatePanel(string name, int sort = 1)
    {
		UIPanelBase panelBase = null;
        if (_acvtive_panels.TryGetValue(name, out panelBase) && panelBase != null)
        {
            panelBase.gameObject.SetActive(true);
            return panelBase;
        }
		
        var panel = UIResourceLoader.m_instance.LoadPanel(name);
        if (sort >= 0 && sort < _sort_panels.Count)
        {
            panel.transform.SetParent(_sort_panels[sort], false);
        }
        else
        {
            if (_sort_panels.Count >= 2)
            {
                panel.transform.SetParent(_sort_panels[1], false);
            }
            else if (_sort_panels.Count > 0)
            {
                panel.transform.SetParent(_sort_panels[0], false);
            }
        }
		
        if(panel == null)
        {
			Debug.LogError("UIPanel Not-Found:" + name);
			return null;
        }
		else
		{
			panelBase = panel.gameObject.GetComponent<UIPanelBase>();
			_acvtive_panels[name] = panelBase;
			return panelBase;
		}
    }
	
	public UIComponentBase LoadComponent(string name, Transform parent, int sort = 1)
	{
		var component = UIResourceLoader.m_instance.LoadComponent(name);
        if (component == null)
        {
			Debug.LogError("UIComponent Not-Found:" + name);
			return null;
        }
		else
		{
			if(parent != null)
			{
				component.transform.SetParent(parent);
			}
            else if(sort >= 0 && sort < _sort_panels.Count)
            {
                    component.transform.SetParent(_sort_panels[sort], false);
                }
            else
            {
                    if (_sort_panels.Count >= 2)
                    {
                        component.transform.SetParent(_sort_panels[1], false);
                    }
                    else if (_sort_panels.Count > 0)
                    {
                        component.transform.SetParent(_sort_panels[0], false);
                    }
            }
            UIComponentBase componentBase = component.gameObject.GetComponent<UIComponentBase>();
			return componentBase;
		}
	}

    public void LoadScene(int i, Transform parent)
    {
        var scene = UIResourceLoader.m_instance.LoadScene(i);
        if (scene == null)
        {
            Debug.LogError("UIScene Not-Found:Scene " + i.ToString());
            return;
        }
        else
        {
            if (parent != null)
            {
                scene.transform.SetParent(parent);
                scene.transform.localPosition = Vector3.zero;
            }
        }
    }
	
	public bool DestroyPanel(string name)
    {
        UIPanelBase panel = null;
        if (_acvtive_panels.TryGetValue(name, out panel) && panel != null)
        {
            UIResourceLoader.m_instance.DestroyPanel(panel.gameObject);
            return true;
        }
        return false;
    }
	
    public bool HidePanel(string name)
    {
        UIPanelBase panel = null;
        if (_acvtive_panels.TryGetValue(name, out panel) && panel != null)
        {
            panel.gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}
