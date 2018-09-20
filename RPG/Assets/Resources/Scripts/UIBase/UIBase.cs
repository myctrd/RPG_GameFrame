using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour {

    public EventListener onAddedToStage { set; get; }

    public bool IsDestroy { get; private set; }

    public virtual void Hide() { }

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
        var child = transform.GetComponentsInChildren<UIMarker>(true);
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

public class EventListener
{
}