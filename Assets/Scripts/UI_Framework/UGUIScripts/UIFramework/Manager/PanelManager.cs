using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������������ջ���洢UI,��ջ��ջ��󶨻�ȡ���¼���
/// </summary>
public class PanelManager
{
    private Stack<BasePanel> stackPanel;
    //������ȡUI
    private ThisUIManager uIManager;
    //��ǰ���
    public BasePanel panel;
    public PanelManager() {
        stackPanel = new Stack<BasePanel>();
        uIManager = new ThisUIManager();
    }
    /// <summary>
    /// �ܽ�
    /// Push����һ��panel��ִ��Pause��ͣ��������ǽpanel��ִ��Enter��ջ����
    /// Pop������ʱջ��panelִ��Exit�˳��������µ�ջ����ִ��Resume��������
    /// </summary>

    //UI��ջ���� ����ʾ��һ�����
    public void Push(BasePanel nextPanel) {
        if (stackPanel.Count > 0) {
        panel=stackPanel.Peek();//��ǰ������ջ��
            panel.OnPause();//������ͣ��ǰ���
        }
        stackPanel.Push(nextPanel);//��ջ��һ�����
        GameObject panelGO = uIManager.GetSingleUI(nextPanel.UIType);//�����һ�����UI

        //����Ĳ������ڸ��½�ջ��UI�ṩ��ǰ�� ��ͬ���Ķ���� ջ�أ�
        //ThisUIManager PanelManager ��ʵ������������� ���ṩ��ǰ����UITool����
        nextPanel.Initialize(new UITool(panelGO));
        nextPanel.Initialize(this);//�൱�ڸ��������������
        nextPanel.Initialize(uIManager);
        nextPanel.OnEnter();
    }
    public void Pop() {
        if (stackPanel.Count > 0) {
            //stackPanel.Peek().OnExit();//ջ���˳�
            //stackPanel.Pop();//����ջ
            stackPanel.Pop().OnExit();
        }
        if (stackPanel.Count > 0)
        {
            stackPanel.Peek().OnResume();//���ջ�ﻹ��Ԫ�أ���ջ��UI�����м�������
        }
    }
    /// <summary>
    /// ���������壬(�л�����ʱ��,���ڹ���һ��stack���������ջ�������)
    /// </summary>
    public void PopAll() {
        while (stackPanel.Count > 0) {
            stackPanel.Pop().OnExit();
        }
    }
}
