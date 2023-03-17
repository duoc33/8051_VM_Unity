using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI������ ��ȡĳ���Ӷ�������Ĳ���
/// </summary>
public class UITool
{
    //��ǰ����
    GameObject activePanel;
    public UITool(GameObject activePanel)
    {
        this.activePanel = activePanel;
    }
    /// <summary>
    /// ��ǰ����ȡ�������������Tֻ��Ϊcomponet
    /// </summary>
    /// <typeparam name="T">���</typeparam>
    /// <returns></returns>
    public T GetOrAddComponet<T>() where T : Component 
    {
        if (activePanel.GetComponent<T>() == null) {
        activePanel.AddComponent<T>();
        }
        return activePanel.GetComponent<T>();
    }
    /// <summary>
    /// �������Ʋ���һ���Ӷ���
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject FindChildGameObject(string name) {
        Transform[] trans = activePanel.GetComponentsInChildren<Transform>();
        foreach (Transform item in trans)
        {
            if (item.name == name) return item.gameObject;
        }
        Debug.Log($"{activePanel.name}���Ҳ���{name}���Ӷ���");
        return null;
    }
    /// <summary>
    /// �������ƻ��һ���Ӷ������
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <param name="name">�Ӷ�������</param>
    /// <returns></returns>
    public T GetOrAddComponetInChildren<T>(string name)where T:Component
    { 
        GameObject child=FindChildGameObject(name);
        if (child)
        {
            if (child.GetComponent<T>() == null) child.AddComponent<T>();
            return child.GetComponent<T>();
        }
        return null;
    }
}
