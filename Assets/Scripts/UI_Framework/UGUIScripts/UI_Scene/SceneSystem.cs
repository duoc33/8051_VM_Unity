using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 场景的状态管理系统
/// </summary>
public class SceneSystem
{
    //上一个或当前当前场景
    SceneState sceneState;
    //设置当前场景进入
    public void SetScene(SceneState scene)
    {
        //if (sceneState != null) sceneState.OnExit();
        //sceneState = scene;
        //if(sceneState!=null)sceneState.OnEnter();
        //与上面同理
        sceneState?.OnExit();//上一个场景
        sceneState = scene;//把需要设置的场景给过来
        sceneState?.OnEnter();
    }
}
