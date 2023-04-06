using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public KeyButtonInfo kInfo;
    public void OnPointerDown(PointerEventData eventData)
    {
        kInfo.DownHandler(this.transform);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        kInfo.UpHandler(this.transform);
    }
}
