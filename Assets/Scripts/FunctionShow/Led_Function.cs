using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Led_Function : MonoBehaviour
{
    [SerializeField]
    private Material ChangeMaterial;
    [SerializeField]
    private Material OriginalMaterial;
    private List<Transform> leds;
    private bool[] isShow;
    private Dictionary<int,byte> IntToHexDic;
    void Start()
    {
        IntToHexDic = new Dictionary<int, byte>
        {
            { 0, 0x1 },
            { 1, 0x2 },
            { 2, 0x4 },
            { 3, 0x8 },
            {  4 ,0x10},
            {  5 ,0x20},
            {  6 ,0x40},
            {  7 ,0x80},
        };
        leds = new List<Transform>();
        isShow =new bool[8];
        foreach (Transform t in transform)
        {
            leds.Add(t);
        }
    }
    public void LED_Light_On(Queue<byte> bytes,bool isRunning) 
    {
        if (bytes.Count > 0 && isRunning)
        {
            byte p2 = bytes.Dequeue();
            LED_Show(p2);
        }
        else if (!isRunning) 
        {
            if (bytes.Count > 0)
                bytes.Clear();
            foreach (var item in leds)
            {
                if (item.GetComponent<Light>().enabled) 
                {
                    item.GetComponent<Renderer>().material = OriginalMaterial;
                    item.GetComponent<Light>().enabled = false;
                }
            }
        }
    }
    private void LED_Show(byte p2)
    {
        for (int i = 0; i < isShow.Length; i++)
        {
            isShow[i] = (p2 & IntToHexDic[i]) == 0 ? true : false;
            if (isShow[i])
            {
                if (!leds[i].GetComponent<Light>().enabled)
                    leds[i].GetComponent<Light>().enabled = true;
                if (!leds[i].GetComponent<Renderer>().material.name.Equals("red"))
                    leds[i].GetComponent<Renderer>().material = ChangeMaterial;
            }
            else
            {
                if (leds[i].GetComponent<Light>().enabled)
                    leds[i].GetComponent<Light>().enabled = false;
                if (!leds[i].GetComponent<Renderer>().material.name.Equals("Deng"))
                    leds[i].GetComponent<Renderer>().material = OriginalMaterial;
            }
        }
    }
}
