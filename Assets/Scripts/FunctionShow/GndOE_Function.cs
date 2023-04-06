using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class GndOE_Function : MonoBehaviour,IPointerClickHandler
{
    public UnityAction<bool> action_OE;
    private bool is_C595_On = false;
    [SerializeField]
    private GameObject OE_GND;
    [SerializeField]
    private GameObject OE_VCC;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!is_C595_On) 
        {
            is_C595_On = true;
            this.transform.SetParent(OE_GND.transform);
            this.transform.localPosition = Vector3.zero;
            action_OE?.Invoke(is_C595_On);
        }
        else 
        {
            is_C595_On = false;
            this.transform.SetParent(OE_VCC.transform);
            this.transform.localPosition = Vector3.zero;
            action_OE?.Invoke(is_C595_On);
        }
    }
    public void Close() 
    {
        is_C595_On = false;
        this.transform.SetParent(OE_VCC.transform);
        this.transform.localPosition = Vector3.zero;
        action_OE?.Invoke(is_C595_On);
    }
}
