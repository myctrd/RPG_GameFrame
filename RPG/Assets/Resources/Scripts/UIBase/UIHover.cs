using UnityEngine;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string infoEnter, infoExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventManager.Broadcast("Hover." + infoEnter);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.Broadcast("Hover." + infoExit);
    }
}
