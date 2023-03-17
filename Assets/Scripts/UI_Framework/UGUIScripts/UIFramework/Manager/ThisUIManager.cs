using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 管理UI的创建与销毁，存储UI所有信息
/// </summary>
public class ThisUIManager
{
    /// <summary>
    /// 存储所有UI信息，每一个UI都会对应一个GameObject
    /// </summary>
    private Dictionary<UIType, GameObject> dicUI;
    //构造函数里初始化字典
    public ThisUIManager() { dicUI = new Dictionary<UIType, GameObject>(); }
    /// <summary>
    /// 获取一个UI对象
    /// </summary>
    /// <param name="type">UI信息</param>
    /// <returns></returns>
    public GameObject GetSingleUI(UIType type) {
        GameObject parent = GameObject.Find("Canvas");
        if (parent == null) { Debug.Log("canvas null");return null; }
        if (dicUI.ContainsKey(type)) {
            if (dicUI[type].activeSelf == false)
                dicUI[type].SetActive(true);
            return dicUI[type];
        }
        //如果没有则从资源文件夹里实例一个，并且将画布设置为它的父级
        GameObject uiObject = GameObject.Instantiate(Resources.Load<GameObject>(type.Path), parent.transform);
        //让他名字就等与UI信息的名字
        uiObject.name = type.Name;
        dicUI.Add(type, uiObject);//加进字典
        return uiObject;
    }
    //回收UI
    public void CollectUI(UIType type) {
        if (dicUI.ContainsKey(type))
        {
            dicUI[type].gameObject.SetActive(false);
        }
    }
    //销毁UI
    public void DestroyUI(UIType type) {
        if (dicUI.ContainsKey(type)) {
            GameObject.Destroy(dicUI[type]);
            dicUI.Remove(type);
        }
    }


}
