using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// P25脉冲，无源蜂鸣器
/// </summary>
public class Beer_Function : MonoBehaviour
{
    //C:\Users\35754\Desktop\单片机\codefile\蜂鸣器\sound.hex
    AudioSource BeerSrc;
    void Start()
    {
        BeerSrc = this.transform.GetComponent<AudioSource>();
    }
    public void BeerPlay(Queue<byte> bytes) 
    {
        if (bytes.Count >= 2) 
        {
            BeerHit(bytes);
        }
    }
    public void Close() 
    {
        if (BeerSrc.isPlaying)
        {
            BeerSrc.Stop();
        }
    }
    byte data1;
    byte data2;
    bool isOn1;
    bool isOn2;
    void BeerHit(Queue<byte> bytes) 
    {
        data1 = bytes.Dequeue();
        data1 >>= 5;
        isOn1 = (data1 & 0x1) != 0 ? true : false;
        data2 = bytes.Dequeue();
        data2 >>= 5;
        isOn2 = (data2 & 0x1) != 0 ? true : false;
        if (isOn1 ^ isOn2)// 00 11
        {
            if (BeerSrc.isPlaying)
                BeerSrc.Stop();
            BeerSrc.Play();
        }
        else
        {
            BeerSrc.Stop();
        }
    }
}
