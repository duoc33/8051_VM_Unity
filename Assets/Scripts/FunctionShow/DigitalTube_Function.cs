using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 共阴数码管，高电平有效
/// </summary>
public class DigitalTube_Function : MonoBehaviour
{
    //C:\Users\35754\Desktop\单片机\codefile\静态数码管\main.hex
    //FF2000
    Transform[] digitalTubes = new Transform[8];
    private float switchTime = 0.35f;
    private Coroutine[] switchCoroutines = new Coroutine[64];
    private Color TargetColor = new Color32(255,32,0,255);
    private Color CurrentColor = new Color32(255, 255, 255, 255);
    private Material[] materials = new Material[64];
    private Renderer[] renderers = new Renderer[64];
    void Start()
    {
        renderers = this.gameObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < transform.childCount; i++)
        {
            digitalTubes[i] = transform.GetChild(i);
        }
        for (int i = 0; i < 64; i++)
        {
            materials[i] = renderers[i].sharedMaterial;
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
            _234index = VM_8051_Mono.Instance.ReadSFR_Action(0xA0);//P2
            _segInfo = VM_8051_Mono.Instance.ReadSFR_Action(0x80);//P0
            //读取P2口的 234位
            _234index = (byte)(_234index & 0x1C);//0001 1100
            _234index >>= 2;
            //判断数码管与上次是否为同一个，不是则消除上一个的影响
            if (lastIndex != _234index) digitalTubes_Rec(lastIndex);
            lastIndex = _234index;//111
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
                materials[index*8+i].SetColor("_Color", TargetColor);
            }
            else 
            {
                materials[index * 8 + i].SetColor("_Color", CurrentColor);
            }
        }
    }
    private void digitalTubes_Rec(byte index)
    {
        for (int i = 0; i < 8; i++)
        {
            SwitchMaterial(materials[index * 8 + i],index * 8 + i);
            //materials[index*8+i].SetColor("_Color", CurrentColor);//没有Lerp
        }
    }
    private void SwitchMaterial(Material segMat,int index)
    {
        if (segMat.color == TargetColor) 
        {
            switchCoroutines[index] = StartCoroutine(SwitchCoroutine(segMat,index));
        }
        else if (switchCoroutines[index] != null)
        {
            StopCoroutine(switchCoroutines[index]);
            segMat.SetColor("_Color", TargetColor);
            switchCoroutines[index] = StartCoroutine(SwitchCoroutine(segMat, index));
        }
    }
    private IEnumerator SwitchCoroutine(Material segMat,int index)
    {
        float elapsedTime = 0f;
        while (elapsedTime < switchTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / switchTime);
            Color newColor = Color.Lerp(TargetColor, CurrentColor, t);
            segMat.SetColor("_Color", newColor);
            yield return null;
        }
        switchCoroutines[index] = null;
    }
    public void Close() 
    { 
        foreach (Transform item in digitalTubes)
        {
            foreach (Transform _item in item)
            {
                _item.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", CurrentColor);
            }
        }
    }
}

// public class DigitalTube_Function : MonoBehaviour{
//     //数码管位选模型
//     Transform[] digitalTubes = new Transform[8];
//     //数码管所有段选材质
//     private Material[] materials = new Material[64];
//     //渐变时间
//     private float switchTime = 0.4f;
//     //颜色变化的协程函数
//     private Coroutine[] switchCoroutines = new Coroutine[64];
//     //目标颜色
//     private Color TargetColor = new Color32(255, 32, 0, 255);
//     //当前颜色(默认颜色)
//     private Color CurrentColor = new Color32(255, 255, 255, 255);
//     //记录上次的显隐，如果38译码器变了，则变
//     byte lastIndex = 0;
//     //数码管位选
//     byte _234index = 0;
//     //数码管段选信息
//     byte _segInfo = 0;
//     //数码管功能入口协程函数
//     IEnumerator digitalTubes_Get(){
//         //运行时进行标志位
//         while (VM_Runtime.Instance.isRunning){
//             //读取P2口,234位数据信息
//             _234index = Get_P2_234(0xA0);
//             //读取P0口数据信息
//             _segInfo = Get_P0(0x80);
//             //判断数码管位选与上次是否为同一个，不是则消除上一个的影响
//             if (lastIndex != _234index) digitalTubes_Rec(lastIndex);
//             lastIndex = _234index;//111
//             //读取P0口数码管的段选位，并显示相关逻辑
//             digitalTubes_Seg_On(_234index, _segInfo);
//             yield return null;
//         }
//     }
//     //消除上一次的数码管位选信息，用到颜色的插值运算
//     private void digitalTubes_Rec(byte index){
//         for (int i = 0; i < 8; i++){
//             SwitchMaterial(materials[index * 8 + i], index * 8 + i);
//         }
//     }
//     private void SwitchMaterial(Material segMat, int index){
//         //判断要消隐的颜色是否等于目标颜色
//         if (segMat.color == TargetColor){
//             //进行插值运算实现渐变消隐
//             switchCoroutines[index] = StartCoroutine(SwitchCoroutine(segMat, index));
//         }
//         //如果当前段选仍在协程中执行，则进行重新插值运算
//         else if (switchCoroutines[index] != null){
//             segMat.SetColor("_Color", TargetColor);
//             switchCoroutines[index] = StartCoroutine(SwitchCoroutine(segMat, index));
//         }
//     }
//     //颜色插值运算程序
//     private IEnumerator SwitchCoroutine(Material segMat, int index){
//         //从0开始计算时间Tn
//         float elapsedTime = 0f;
//         while (elapsedTime < switchTime){
//             elapsedTime += Time.deltaTime;
//             //计算Tn/Tc
//             float t = Mathf.Clamp01(elapsedTime / switchTime);
//             Color newColor = Color.Lerp(TargetColor, CurrentColor, t);
//             //将段选材质赋值新获得的插值颜色
//             segMat.SetColor("_Color", newColor);
//             yield return null;
//         }
//         switchCoroutines[index] = null;
//     }
// }
