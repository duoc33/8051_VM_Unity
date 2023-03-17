using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI管理工具 获取某个子对象组件的操作
/// </summary>
public class UITool
{
    //当前活动面板
    GameObject activePanel;
    public UITool(GameObject activePanel)
    {
        this.activePanel = activePanel;
    }
    /// <summary>
    /// 当前面板获取或者组件，限制T只能为componet
    /// </summary>
    /// <typeparam name="T">组件</typeparam>
    /// <returns></returns>
    public T GetOrAddComponet<T>() where T : Component 
    {
        if (activePanel.GetComponent<T>() == null) {
        activePanel.AddComponent<T>();
        }
        return activePanel.GetComponent<T>();
    }
    /// <summary>
    /// 根据名称查找一个子对象
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject FindChildGameObject(string name) {
        Transform[] trans = activePanel.GetComponentsInChildren<Transform>();
        foreach (Transform item in trans)
        {
            if (item.name == name) return item.gameObject;
        }
        Debug.Log($"{activePanel.name}里找不到{name}的子对象");
        return null;
    }
    /// <summary>
    /// 根据名称获得一个子对象组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="name">子对象名称</param>
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
