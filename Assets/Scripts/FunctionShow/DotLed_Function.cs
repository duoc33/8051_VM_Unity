using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// P0  P3.4 SER  P3.5 RCLK  P3.6 SRCLK  0111 0000 0x70
/// </summary>
public class DotLed_Function : MonoBehaviour
{
    byte A8_A1;//行
    byte P0_info;//列
    int row;//行亮为1
    int col;//列
    bool[] isLightOn =new bool[64];
    bool value;//临时变量
    int index;//临时索引
    Light[] Dotlights;//所有灯光
    void Start()
    {
        Dotlights = transform.GetComponentsInChildren<Light>();
    }
    public void DotLed_On(Queue<byte> c595_register_bytes,Queue<byte> p0_datas,bool is_C595_On) 
    {
        if (c595_register_bytes.Count > 0&&p0_datas.Count>0&&is_C595_On)
        {
            A8_A1 = c595_register_bytes.Dequeue();
            P0_info = p0_datas.Dequeue();
            DotLed_light(A8_A1,P0_info);
        }
        else if (!is_C595_On) 
        {
            Close();
        }
    }
    //0000 0000  1111 1111
    private void DotLed_light(byte _A8_A1,byte _P0_info) 
    {
        CheckBits(_A8_A1, _P0_info);
        for (int i = 0; i < Dotlights.Length; i++)
        {
            Dotlights[i].enabled = isLightOn[i];
        }
    }
    private void CheckBits(byte a, byte b)
    {
        for (int i = 0; i < 8; i++)
        {
            row = (byte)(a & (1 << i)); // 提取A中的第i位
            for (int j = 0; j < 8; j++)
            {
                col = (byte)(b & (1 << j)); // 提取B中的第j位
                index = i * 8 + j;
                value = (row != 0) && (col == 0x00);
                isLightOn[index] = value;
            }
        }
    }
    public void Close() 
    {
        foreach (Light item in transform.GetComponentsInChildren<Light>())
        {
            item.enabled = false;
        } 
    }
}
