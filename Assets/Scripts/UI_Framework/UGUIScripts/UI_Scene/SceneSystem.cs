using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������״̬����ϵͳ
/// </summary>
public class SceneSystem
{
    //��һ����ǰ��ǰ����
    SceneState sceneState;
    //���õ�ǰ��������
    public void SetScene(SceneState scene)
    {
        //if (sceneState != null) sceneState.OnExit();
        //sceneState = scene;
        //if(sceneState!=null)sceneState.OnEnter();
        //������ͬ��
        sceneState?.OnExit();//��һ������
        sceneState = scene;//����Ҫ���õĳ���������
        sceneState?.OnEnter();
    }
}
