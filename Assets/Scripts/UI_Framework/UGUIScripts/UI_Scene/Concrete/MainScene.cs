using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : SceneState
{
    readonly string sceneName = "mainScene";
    PanelManager panelManager;//全局变量会保留上一次的引用
    public override void OnEnter()
    {
        panelManager = new PanelManager();//UI管理器会跟着new出来
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            SceneManager.LoadScene(sceneName);
            SceneManager.sceneLoaded += SceneLoaded;
        }
        else
        {
            Debug.Log($"{sceneName} 加载完毕");
            //panelManager.Push(new SecondMainPanel());
            GameRoot.Instance.SetAction(panelManager.Push);
        }
    }
    /// <summary>
    /// 场景加载后执行的方法
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="load"></param>
    private void SceneLoaded(Scene scene, LoadSceneMode load)
    {
        //panelManager.Push(new SecondMainPanel());
        GameRoot.Instance.SetAction(panelManager.Push);
        Debug.Log($"{sceneName} 加载完毕");
    }
    public override void OnExit()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        panelManager.PopAll();
    }
}
