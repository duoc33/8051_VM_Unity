using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VM_KeyButton_Monitor : MonoBehaviour
{
    //individualKey 0xB0 

    //0x90
    //p10 ~p13从右往左  p14 ~p17从下到上
    //RectKey s1 0111 0111 0x77 s2 0111 1011 0x7b s3 0111 1101 0x7d s4 0111 1110 0x7e
    //        s5 1011 0111 0xb7
    //        s9 1101 0111 0xd7 
    //       s13 1110 0111 0xe7
    //       i%4 =0 1 2 3 0xf&0x1<<3-i
    //       (byte)(((0xf0 ^ (0x10<<(3-i/4)))) | (0x0f ^ (0x01 << (3-i%4))))
    //       
    //       0111 1111 ^ 0000 1000  =  0111 0111;  0111 1111^ 0000 0100 = 0111 1011;

    //记录当前按下键位的Key值
    public static byte rectInfo;
    public static byte aloneKeyInfo;
    [SerializeField]
    private Transform individualKey;
    [SerializeField]
    private Transform RectKey;
    [SerializeField]
    private Transform Reset;
    void Start()
    {
        InitButton();
        VM_8051_Mono.Instance.has_Input_P1_data_handler +=RectKeyDetect;
        VM_8051_Mono.Instance.has_Input_Alone_handler += AloneKeyInfo;
    }
    void OnDestroy() 
    {
        VM_8051_Mono.Instance.has_Input_P1_data_handler -= RectKeyDetect;
        VM_8051_Mono.Instance.has_Input_Alone_handler -= AloneKeyInfo;
    }
    private void InitButton() 
    {
        if (Reset.GetComponent<KeyButton>() == null)
        {
            KeyButton myKey = Reset.gameObject.AddComponent<KeyButton>();
            myKey.kInfo = new KeyButtonInfo(0, KeyType.Reset);
        }
        for (int i = 0; i < individualKey.childCount; i++)
        {
            if (individualKey.GetChild(i).GetComponent<KeyButton>() == null)
            {
                KeyButton myKey = individualKey.GetChild(i).gameObject.AddComponent<KeyButton>();
                myKey.kInfo = new KeyButtonInfo((byte)(0xB0 + i), KeyType.individual);
            }
        }
        for (int i = 0; i < RectKey.childCount; i++)
        {
            if (RectKey.GetChild(i).GetComponent<KeyButton>() == null)
            {
                KeyButton myKey = RectKey.GetChild(i).gameObject.AddComponent<KeyButton>();
                byte data = (byte)(((0xf0 ^ (0x10 << (3 - i / 4)))) | (0x0f ^ (0x01 << (3 - i % 4))));
                myKey.kInfo =new KeyButtonInfo(data,KeyType.Rect);
            }
        }
    }
    public byte RectKeyDetect()
    {
        return rectInfo;
    }
    public byte AloneKeyInfo() 
    {
        return aloneKeyInfo;
    }
}
