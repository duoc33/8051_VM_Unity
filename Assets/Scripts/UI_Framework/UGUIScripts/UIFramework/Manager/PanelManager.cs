using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 面板管理器，用栈来存储UI,进栈出栈会绑定或取消事件等
/// </summary>
public class PanelManager
{
    private Stack<BasePanel> stackPanel;
    //用来获取UI
    private ThisUIManager uIManager;
    //当前面板
    public BasePanel panel;
    public PanelManager() {
        stackPanel = new Stack<BasePanel>();
        uIManager = new ThisUIManager();
    }
    /// <summary>
    /// 总结
    /// Push：上一个panel会执行Pause暂停操作，挡墙panel会执行Enter入栈操作
    /// Pop：弹出时栈顶panel执行Exit退出操作，新的栈顶会执行Resume继续操作
    /// </summary>

    //UI入栈操作 会显示下一个面板
    public void Push(BasePanel nextPanel) {
        if (stackPanel.Count > 0) {
        panel=stackPanel.Peek();//当前面板等于栈顶
            panel.OnPause();//并且暂停当前面板
        }
        stackPanel.Push(nextPanel);//入栈下一个面板
        GameObject panelGO = uIManager.GetSingleUI(nextPanel.UIType);//获得下一个面板UI

        //下面的操作属于给新进栈的UI提供当前的 （同样的对象池 栈池）
        //ThisUIManager PanelManager 其实这两个单例最好 并提供当前面板的UITool功能
        nextPanel.Initialize(new UITool(panelGO));
        nextPanel.Initialize(this);//相当于更新这个面板管理器
        nextPanel.Initialize(uIManager);
        nextPanel.OnEnter();
    }
    public void Pop() {
        if (stackPanel.Count > 0) {
            //stackPanel.Peek().OnExit();//栈顶退出
            //stackPanel.Pop();//并出栈
            stackPanel.Pop().OnExit();
        }
        if (stackPanel.Count > 0)
        {
            stackPanel.Peek().OnResume();//如果栈里还有元素，则栈顶UI面板进行继续操作
        }
    }
    /// <summary>
    /// 清除所有面板，(切换场景时用,由于公用一个stack，如果不清空会有问题)
    /// </summary>
    public void PopAll() {
        while (stackPanel.Count > 0) {
            stackPanel.Pop().OnExit();
        }
    }
}
