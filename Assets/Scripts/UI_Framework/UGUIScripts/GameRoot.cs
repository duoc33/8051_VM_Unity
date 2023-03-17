using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ����ȫ�� ���г�ʼ�����л�����(���ж�PanelManager�ĳ�ʼ��)�� 
/// ��Ϊί����һ��ÿ��������Push pop���������Խ���UI���֮��Ҳ���ǳ���ת������ʱʹ��
/// ����
/// </summary>
public class GameRoot : MonoBehaviour
{
    //���������Ϊ������ί��
    //���Ը�����UIPanel���ڼ�����Ϻ�����˳�ǰ
    //����������UI���֮����Push . pop�ȷ���(���س���ǰ���˳�����ǰ�Ƚ������)
    //GameRoot.Instance.SetAction(panelManager.Push);
    //����ֱ���ڱ�����ʹ��Push.Invoke(new StartPanel());
    public UnityAction<BasePanel> Push { get; private set; }
    public void SetAction(UnityAction<BasePanel> push)
    {
        Push = push;
    }
    //�����ⲿ���Ի�ȡ�������޸�
    public SceneSystem system { get; private set; }
    public static GameRoot Instance { get; private set; }//�����һ�ֵ�����д��
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
        system =new SceneSystem();
        DontDestroyOnLoad(gameObject);//������ת������������
    }
    private void Start()
    {
        system.SetScene(new StartScene());//��Ϸһ��ʼ����һ�����������һ���ó���ʵ����Ӧ��panelManager UIManager �Ͱ��¼���
    }
}
