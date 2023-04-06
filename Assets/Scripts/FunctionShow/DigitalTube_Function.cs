using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 共阴数码管，高电平有效
/// </summary>
public class DigitalTube_Function : MonoBehaviour
{
    [SerializeField]
    private Material Red;
    [SerializeField]
    private Material Original;
    Transform[] digitalTubes = new Transform[8];
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            digitalTubes[i] = transform.GetChild(transform.childCount-1-i);
        }
    }
    byte lastIndex = 0;//记录上次的显隐，如果38译码器变了，则变
    byte _234index = 0;
    byte _segInfo = 0;
    public void digitalTubes_light() 
    {
        StartCoroutine(digitalTubes_Get());
    }
    IEnumerator digitalTubes_Get() 
    {
        while (VM_Runtime.Instance.isRunning)
        {
            _234index = VM_8051_Mono.Instance.ReadSFR_Action(0xA0);
            _segInfo = VM_8051_Mono.Instance.ReadSFR_Action(0x80);
            //读取P2口的 234位
            _234index = (byte)(_234index & 0x1C);
            _234index >>= 2;
            //判断数码管与上次是否为同一个，不是则，消除上一个的影响
            if (lastIndex != _234index) digitalTubes_Rec(lastIndex);
            lastIndex = _234index;
            //读取P0口数码管的段选位
            digitalTubes_Seg_On(_234index, _segInfo);
            yield return null;
        }
    }
    private void digitalTubes_Seg_On(byte index,byte info) 
    { 
        for (int i = 0; i < digitalTubes[index].childCount; i++)
        {
            if (((info >> i) & 0x1)==1)
            {
                digitalTubes[index].GetChild(i).GetComponent<Renderer>().material = Red;
            }
            else 
            {
                digitalTubes[index].GetChild(i).GetComponent<Renderer>().material = Original;
            }
        }
    }
    private void digitalTubes_Rec(byte index) 
    {
        foreach (Transform item in digitalTubes[index])
        {
            item.GetComponent<Renderer>().material = Original;
        }
    }
    public void Close() 
    { 
        foreach (Transform item in digitalTubes)
        {
            foreach (Transform _item in item)
            {
                _item.GetComponent<Renderer>().material = Original;
            }
        }
    }
}
