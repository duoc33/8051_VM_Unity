using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有UI面板的父类，包含UI面板的状态信息
/// </summary>
public abstract class BasePanel
{
    /// <summary>
    /// UI信息
    /// </summary>
    public UIType UIType { get; private set; }//同样只能读，在内部进行修改
    /// <summary>
    /// UI管理工具 查找组件
    /// </summary>
    public UITool UITool { get; private set; }
    public BasePanel(UIType uIType) { UIType = uIType; }
    /// <summary>
    /// 面板管理器
    /// </summary>
    public PanelManager PanelManager { get; private set; }

    //UI管理器
    public ThisUIManager UIManager { get; private set; }
    //初始化UI管理器 (更新传递比较恰当)
    public void Initialize(ThisUIManager uIManager) {
        UIManager = uIManager;
    }
    /// <summary>
    /// 初始化UITool 方法重载
    /// </summary>
    /// <param name="uITool"></param>
    public void Initialize(UITool uITool) { 
    this.UITool = uITool;
    }
    //方法重载 初始化面板管理器 在面板管理器中再调用进行更新
    public void Initialize(PanelManager panelManager) {
        PanelManager= panelManager;
    }

    /// <summary>
    /// UI进入时执行的操作，只会执行一次
    /// </summary>
    public virtual void OnEnter() {
    
    }
    //UI暂停时执行的操作(当点了一个UI其他UI是否还能执行)
    public virtual void OnPause() { UITool.GetOrAddComponet<CanvasGroup>().blocksRaycasts = false; }
    //UI继续时执行的操作
    public virtual void OnResume() { UITool.GetOrAddComponet<CanvasGroup>().blocksRaycasts = true; }
    //UI退出时执行的操作
    public virtual void OnExit() {Debug.Log("Destroy this UI");UIManager.DestroyUI(UIType);}

    /// <summary>
    /// 由于PanelManger.Push和Pop在子类用得平凡 所以下面代码表示子类可以简写为方法名
    /// </summary>
    /// <param name="panel"></param>
    public void Push(BasePanel panel)=>PanelManager?.Push(panel);
    public void Pop()=>PanelManager?.Pop();
}
