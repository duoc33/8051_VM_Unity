using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �洢����UI����Ϣ���������ֺ�·��
/// </summary>
public class UIType
{
    //UI����
    public string Name { get; private set; }
    //UI·��(����Resource�ļ���)
    public string Path { get; private set; }
    public UIType(string path) { 
        Path= path;
        Name=path.Substring(path.LastIndexOf('/')+1);//������·�������һ��б��+1��λ�ÿ�ʼ���ַ���
    }
}
