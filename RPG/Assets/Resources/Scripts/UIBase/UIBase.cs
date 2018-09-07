using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour {
    
    public bool IsDestroy { get; private set; }

    public virtual void Close()
    {
        this.Destory();
    }

    private void Destory()
    {
        if (IsDestroy)
        {
            return;
        }

        IsDestroy = true;
    }

    public UIMarker GetChild(string name)
    {
        var child = transform.GetComponentsInChildren<UIMarker>();
        for(int i = 0; i < child.Length; i ++)
        {
            if(child[i].gameObject.name == name)
            {
                return child[i];
            }
        }
        return null;
    }
}
