using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

public class VM_Delegate : MonoBehaviour
{
    #region ���������ֶκͶ���
    private Queue<byte> Led_data = new Queue<byte>();
    [SerializeField]
    private Led_Function led_exe;
    #endregion

    #region ί��ע��
    private void OnEnable()
    {
        VM_8051_Mono.Instance.read_P2_data_handler += Read_P2_LED_Data;
    }
    private void OnDisable()
    {
        VM_8051_Mono.Instance.read_P2_data_handler -= Read_P2_LED_Data;
    }
    #endregion

    #region ִ�лص�
    private void Read_P2_LED_Data(byte P2_Led)
    {
        Led_data.Enqueue(P2_Led);
    }
    #endregion

    #region ִ�з���
    private void Update()
    {
        led_exe.LED_Light_On(Led_data,VM_Runtime.Instance.isRunning);
    }
    #endregion
}

