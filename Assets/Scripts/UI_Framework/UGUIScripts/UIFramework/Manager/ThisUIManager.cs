using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����UI�Ĵ��������٣��洢UI������Ϣ
/// </summary>
public class ThisUIManager
{
    /// <summary>
    /// �洢����UI��Ϣ��ÿһ��UI�����Ӧһ��GameObject
    /// </summary>
    private Dictionary<UIType, GameObject> dicUI;
    //���캯�����ʼ���ֵ�
    public ThisUIManager() { dicUI = new Dictionary<UIType, GameObject>(); }
    /// <summary>
    /// ��ȡһ��UI����
    /// </summary>
    /// <param name="type">UI��Ϣ</param>
    /// <returns></returns>
    public GameObject GetSingleUI(UIType type) {
        GameObject parent = GameObject.Find("Canvas");
        if (parent == null) { Debug.Log("canvas null");return null; }
        if (dicUI.ContainsKey(type)) {
            if (dicUI[type].activeSelf == false)
                dicUI[type].SetActive(true);
            return dicUI[type];
        }
        //���û�������Դ�ļ�����ʵ��һ�������ҽ���������Ϊ���ĸ���
        GameObject uiObject = GameObject.Instantiate(Resources.Load<GameObject>(type.Path), parent.transform);
        //�������־͵���UI��Ϣ������
        uiObject.name = type.Name;
        dicUI.Add(type, uiObject);//�ӽ��ֵ�
        return uiObject;
    }
    //����UI
    public void CollectUI(UIType type) {
        if (dicUI.ContainsKey(type))
        {
            dicUI[type].gameObject.SetActive(false);
        }
    }
    //����UI
    public void DestroyUI(UIType type) {
        if (dicUI.ContainsKey(type)) {
            GameObject.Destroy(dicUI[type]);
            dicUI.Remove(type);
        }
    }


}
