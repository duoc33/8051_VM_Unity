using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����״̬
/// </summary>
public abstract class SceneState
{
    //��������ʱִ�еĲ���(���¼���)
    public abstract void OnEnter();
    //�����˳�ʱִ�еĲ���
    public abstract void OnExit();
}
