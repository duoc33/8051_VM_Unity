using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存储单个UI的信息，包括名字和路径
/// </summary>
public class UIType
{
    //UI名字
    public string Name { get; private set; }
    //UI路径(放在Resource文件夹)
    public string Path { get; private set; }
    public UIType(string path) { 
        Path= path;
        Name=path.Substring(path.LastIndexOf('/')+1);//从最后该路径的最后一个斜杆+1的位置开始的字符串
    }
}
