using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 管理全局 进行初始化，切换场景(里有对PanelManager的初始化)等 
/// 因为委托了一个每个场景的Push pop方法，可以进行UI框架之外也就是场景转换中随时使用
/// 单例
/// </summary>
public class GameRoot : MonoBehaviour
{
    //定义面板类为参数的委托
    //可以更灵活穿插UIPanel，在加载完毕后或者退出前
    //这样可以在UI框架之外用Push . pop等方法(加载场景前，退出场景前等进行与绑定)
    //GameRoot.Instance.SetAction(panelManager.Push);
    //例如直接在本类中使用Push.Invoke(new StartPanel());
    public UnityAction<BasePanel> Push { get; private set; }
    public void SetAction(UnityAction<BasePanel> push)
    {
        Push = push;
    }
    //都是外部可以获取但不能修改
    public SceneSystem system { get; private set; }
    public static GameRoot Instance { get; private set; }//这就是一种单例的写法
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
        system =new SceneSystem();
        DontDestroyOnLoad(gameObject);//场景跳转让它不被销毁
    }
    private void Start()
    {
        system.SetScene(new StartScene());//游戏一开始加载一个场景，并且会给该场景实例相应的panelManager UIManager 和绑定事件等
    }
}
