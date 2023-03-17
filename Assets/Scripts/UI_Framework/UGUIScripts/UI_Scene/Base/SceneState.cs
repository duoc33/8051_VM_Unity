using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 场景状态
/// </summary>
public abstract class SceneState
{
    //场景进入时执行的操作(绑定事件等)
    public abstract void OnEnter();
    //场景退出时执行的操作
    public abstract void OnExit();
}
