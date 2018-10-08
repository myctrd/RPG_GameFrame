using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string infoEnter, infoExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Dictionary<string, object> p = new Dictionary<string, object>();
        p.Add("subInfo", gameObject.name);
        EventManager.Broadcast("Hover." + infoEnter, p);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.Broadcast("Hover." + infoExit);
    }
}
