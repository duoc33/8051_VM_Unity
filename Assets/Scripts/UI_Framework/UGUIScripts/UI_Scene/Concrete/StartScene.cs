using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ��ʼ����
/// </summary>
public class StartScene : SceneState
{
    readonly string sceneName = "startScene";
    PanelManager panelManager;
    public override void OnEnter()
    {
        //���볡����newһ����������
        panelManager=new PanelManager();
        //�����ǰ�������Ǹó�������
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            SceneManager.LoadScene(sceneName);
            //����������ɺ��һ���¼�������һ����壩
            SceneManager.sceneLoaded += SceneLoaded;
        }
        else {
            //��������ʱ�ó��� Ҳ����ؿ�ʼ���
            //panelManager.Push(new StartPanel());
            GameRoot.Instance.SetAction(panelManager.Push);
        }
    }
    /// <summary>
    /// �������غ�ִ�еķ���
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="load"></param>
    private void SceneLoaded(Scene scene,LoadSceneMode load) 
    {
        //panelManager.Push(new StartPanel());//����һ����ʼ���
        //GameRoot.Instance.SetAction(panelManager.Push);
        //Debug.Log($"{sceneName} �������"); 
    }
    public override void OnExit()
    {
        //�˳�ʱע���¼�
        SceneManager.sceneLoaded -= SceneLoaded;
        //���ջ(����������ᵼ�����⣬������ܰ�)
        panelManager.PopAll();
    }
}
