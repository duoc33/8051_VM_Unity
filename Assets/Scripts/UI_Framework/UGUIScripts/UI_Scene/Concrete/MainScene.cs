using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : SceneState
{
    readonly string sceneName = "mainScene";
    PanelManager panelManager;//ȫ�ֱ����ᱣ����һ�ε�����
    public override void OnEnter()
    {
        panelManager = new PanelManager();//UI�����������new����
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            SceneManager.LoadScene(sceneName);
            SceneManager.sceneLoaded += SceneLoaded;
        }
        else
        {
            Debug.Log($"{sceneName} �������");
            //panelManager.Push(new SecondMainPanel());
            GameRoot.Instance.SetAction(panelManager.Push);
        }
    }
    /// <summary>
    /// �������غ�ִ�еķ���
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="load"></param>
    private void SceneLoaded(Scene scene, LoadSceneMode load)
    {
        //panelManager.Push(new SecondMainPanel());
        GameRoot.Instance.SetAction(panelManager.Push);
        Debug.Log($"{sceneName} �������");
    }
    public override void OnExit()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        panelManager.PopAll();
    }
}
