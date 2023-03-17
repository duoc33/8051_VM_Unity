using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 开始场景
/// </summary>
public class StartScene : SceneState
{
    readonly string sceneName = "startScene";
    PanelManager panelManager;
    public override void OnEnter()
    {
        //进入场景先new一个面板管理器
        panelManager=new PanelManager();
        //如果当前场景不是该场景名字
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            SceneManager.LoadScene(sceneName);
            //场景加载完成后的一个事件（加载一个面板）
            SceneManager.sceneLoaded += SceneLoaded;
        }
        else {
            //如果本身就时该场景 也会加载开始面板
            //panelManager.Push(new StartPanel());
            GameRoot.Instance.SetAction(panelManager.Push);
        }
    }
    /// <summary>
    /// 场景加载后执行的方法
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="load"></param>
    private void SceneLoaded(Scene scene,LoadSceneMode load) 
    {
        //panelManager.Push(new StartPanel());//加载一个开始面板
        //GameRoot.Instance.SetAction(panelManager.Push);
        //Debug.Log($"{sceneName} 加载完毕"); 
    }
    public override void OnExit()
    {
        //退出时注销事件
        SceneManager.sceneLoaded -= SceneLoaded;
        //清空栈(不加这个，会导致问题，提高性能吧)
        panelManager.PopAll();
    }
}
