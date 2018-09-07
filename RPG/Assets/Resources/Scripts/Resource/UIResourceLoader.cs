using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class UIResourceLoader : ResourceLoaderBase {
	
	public static UIResourceLoader m_instance;

	Dictionary<string, GameObject> __cache = new Dictionary<string, GameObject>();
	
	void Awake () {
		if(m_instance == null)
		{
			m_instance = this;
		}
	}
	
	public GameObject LoadPanel(string name)
    {
        // find in cache
        GameObject obj = null;
        if (__cache.TryGetValue(name, out obj) && obj != null)
        {

        }
        else
        {
            //missing cache
            obj = this.Load<GameObject>("UIPrefabs/UIPanels/" + name + ".prefab");
            __cache[name] = obj;
        }

        if (obj != null)
        {
            var ret = GameObject.Instantiate<GameObject>(obj, null, false);
            ret.gameObject.name = name;
            return ret;
        }
        return null;
    }

    public string [] LoadDialog(string fileName)
    {
        string[] dialogs;
        dialogs = this.LoadFile(fileName);
        if (dialogs != null)
        {
            return dialogs;
        }
        return null;
    }
	
	public GameObject LoadComponent(string name)
    {
        // find in cache
        GameObject obj = null;
		obj = this.Load<GameObject>("UIPrefabs/UIComponents/" + name + ".prefab");

        if (obj != null)
        {
            var ret = GameObject.Instantiate<GameObject>(obj, null, false);
            ret.gameObject.name = name;
            return ret;
        }
        return null;
    }

    public GameObject LoadScene(int i)
    {
        // find in cache
        string name = "scene_" + i.ToString();
        GameObject obj = null;
        obj = this.Load<GameObject>("UIPrefabs/UIScenes/" + name + ".prefab");

        if (obj != null)
        {
            var ret = GameObject.Instantiate<GameObject>(obj, null, false);
            ret.gameObject.name = name;
            return ret;
        }
        return null;
    }

    public void DestroyPanel(GameObject panel)
    {
        GameObject.Destroy(panel);
    }
}
