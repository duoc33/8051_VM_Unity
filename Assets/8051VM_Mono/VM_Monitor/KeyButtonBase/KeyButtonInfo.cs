using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyButtonInfo
{
    public byte addr;
    private KeyType keyType;
    public KeyButtonInfo(byte _addr,KeyType _keyType) 
    {
        addr = _addr;
        keyType = _keyType;
    }
    public void DownHandler(Transform tf) 
    { 
        switch (keyType)
        {
            case KeyType.Reset:
                {
                    VM_8051_Mono.Instance.Reset();
                    if (tf.localPosition.y != 0.6f)
                        tf.localPosition += new Vector3(0, 0.1f, 0);
                    break;
                }
            case KeyType.individual:
                {
                    //VM_8051_Mono.Instance.WriteSFR_BIT_ACTION(addr, 0);
                    VM_KeyButton_Monitor.aloneKeyInfo = addr;
                    VM_8051_Mono.Instance.isInputAloneKey = true;
                    if (tf.localPosition.z != -0.1f)
                        tf.localPosition += new Vector3(0, 0, -0.1f);
                    break;
                }
            case KeyType.Rect:
                {
                    VM_KeyButton_Monitor.rectInfo = this.addr;
                    VM_8051_Mono.Instance.isInput = true;
                    if(tf.transform.localPosition.y!=0.6f)
                        tf.transform.localPosition += new Vector3(0, 0.1f, 0);
                    break;
                }
        }
    }
    public void UpHandler(Transform tf) 
    {
        switch (keyType)
        {
            case KeyType.Reset:
                {
                    if (tf.localPosition.y != 0.5f)
                        tf.localPosition += new Vector3(0, -0.1f, 0);
                    break;
                }
            case KeyType.individual:
                {
                    //VM_8051_Mono.Instance.WriteSFR_BIT_ACTION(addr, 1);
                    VM_8051_Mono.Instance.isInputAloneKey = false;
                    if (tf.localPosition.z != 0.0f)
                        tf.localPosition += new Vector3(0, 0, 0.1f);
                    break;
                }
            case KeyType.Rect:
                {
                    VM_8051_Mono.Instance.isInput = false;
                    if (tf.transform.localPosition.y != 0.5f)
                        tf.transform.localPosition += new Vector3(0, -0.1f, 0);
                    break;
                }
        }
    }
}
public enum KeyType 
{ 
    Reset,
    individual,
    Rect,
}
