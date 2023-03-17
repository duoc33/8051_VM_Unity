using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����UI���ĸ��࣬����UI����״̬��Ϣ
/// </summary>
public abstract class BasePanel
{
    /// <summary>
    /// UI��Ϣ
    /// </summary>
    public UIType UIType { get; private set; }//ͬ��ֻ�ܶ������ڲ������޸�
    /// <summary>
    /// UI������ �������
    /// </summary>
    public UITool UITool { get; private set; }
    public BasePanel(UIType uIType) { UIType = uIType; }
    /// <summary>
    /// ��������
    /// </summary>
    public PanelManager PanelManager { get; private set; }

    //UI������
    public ThisUIManager UIManager { get; private set; }
    //��ʼ��UI������ (���´��ݱȽ�ǡ��)
    public void Initialize(ThisUIManager uIManager) {
        UIManager = uIManager;
    }
    /// <summary>
    /// ��ʼ��UITool ��������
    /// </summary>
    /// <param name="uITool"></param>
    public void Initialize(UITool uITool) { 
    this.UITool = uITool;
    }
    //�������� ��ʼ���������� �������������ٵ��ý��и���
    public void Initialize(PanelManager panelManager) {
        PanelManager= panelManager;
    }

    /// <summary>
    /// UI����ʱִ�еĲ�����ֻ��ִ��һ��
    /// </summary>
    public virtual void OnEnter() {
    
    }
    //UI��ͣʱִ�еĲ���(������һ��UI����UI�Ƿ���ִ��)
    public virtual void OnPause() { UITool.GetOrAddComponet<CanvasGroup>().blocksRaycasts = false; }
    //UI����ʱִ�еĲ���
    public virtual void OnResume() { UITool.GetOrAddComponet<CanvasGroup>().blocksRaycasts = true; }
    //UI�˳�ʱִ�еĲ���
    public virtual void OnExit() {Debug.Log("Destroy this UI");UIManager.DestroyUI(UIType);}

    /// <summary>
    /// ����PanelManger.Push��Pop�������õ�ƽ�� ������������ʾ������Լ�дΪ������
    /// </summary>
    /// <param name="panel"></param>
    public void Push(BasePanel panel)=>PanelManager?.Push(panel);
    public void Pop()=>PanelManager?.Pop();
}
