using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

public class VM_Monitor : MonoBehaviour
{
    #region LED
    [SerializeField]
    private Led_Function led_exe;
    private Queue<byte> P2_Led_Q = new Queue<byte>();
    #endregion

    #region 蜂鸣器
    [SerializeField] 
    private Beer_Function Beer_exe;
    private Queue<byte> P2_Beer_Q = new Queue<byte>();
    #endregion

    #region 数码管
    [SerializeField]
    DigitalTube_Function digitalTube_exe;
    #endregion
   
    #region Led点阵
    [SerializeField]
    DotLed_Function dotLed_exe;
    [SerializeField]
    GndOE_Function oe;
    byte _34_SER;//p3.4
    byte _35_RCLK;//p3.5
    byte rclkTmp;//上一次的值
    byte _36_SRCLK;//p3.6
    byte srclkTmp;//上一次的值
    byte tempP3;
    int _c595_rigster_index = 7;
    byte _c595_rigster=0;//存储寄存器
    private bool isGnd_OE = false;
    private Queue<byte> _c595_register_bytes =new Queue<byte>();
    private Queue<byte> P0_DotLed_Q = new Queue<byte>();
    #endregion

    #region Uart串行通信
    [SerializeField]
    private Uart_Function uart_exe;
    private Queue<byte> InputTextChar = new Queue<byte>();//
    private Queue<byte> ReceiveChar = new Queue<byte>();//VM trans
    private byte InputChar;
    #endregion

    #region 委托注册 执行回调 监视VM内部数据
    private void Start()
    {
        VM_Runtime.Instance.CloseAction += ClearMono;
        VM_Runtime.Instance.StartAction += StartMono;
        VM_8051_Mono.Instance.read_P2_data_handler += Read_P2_Data;
        VM_8051_Mono.Instance.read_P3_456_data_handler += Read_P3_456_data;
        VM_8051_Mono.Instance.read_P3_456_data_bit_handler +=Read_P3_456_bit_data;
        VM_8051_Mono.Instance.Sbuf_Transmit_handler +=WriteSbuf;
        VM_8051_Mono.Instance.Sbuf_Receive_handler +=ReadSbuf;
        oe.action_OE += GetOE_ON;
    }
    private void OnDestroy()
    {
        VM_Runtime.Instance.CloseAction -= ClearMono;
        VM_Runtime.Instance.StartAction -= StartMono;
        VM_8051_Mono.Instance.read_P2_data_handler -= Read_P2_Data;
        VM_8051_Mono.Instance.read_P3_456_data_handler -= Read_P3_456_data;
        VM_8051_Mono.Instance.read_P3_456_data_bit_handler -=Read_P3_456_bit_data;
        VM_8051_Mono.Instance.Sbuf_Transmit_handler -=WriteSbuf;
        VM_8051_Mono.Instance.Sbuf_Receive_handler -=ReadSbuf;
        oe.action_OE -= GetOE_ON;
        ClearMono();
    }
    private void WriteSbuf(byte data) 
    {
        if(data ==0)return;
        ReceiveChar.Enqueue(data);
        ReceiveChar.TrimExcess();
    }
    private byte ReadSbuf() 
    {
        if (InputTextChar.Count > 0) 
        {
            InputChar = InputTextChar.Dequeue();
            Debug.Log((char)InputChar);
            return InputChar;
        }
        return 0;
    }
    private void Read_P2_Data(byte P2_data)
    {
        P2_Led_Q.Enqueue(P2_data);
        P2_Beer_Q.Enqueue(P2_data);
        P2_Led_Q.TrimExcess();
        P2_Beer_Q.TrimExcess();
    }
    private void Read_P3_456_data(byte p3_data,byte p0_data) 
    {
        if (isGnd_OE)
        {
            tempP3 = (byte)(p3_data & 0x70);
            _35_RCLK = (byte)((tempP3 >> 5) & 0x1);
            _36_SRCLK = (byte)(tempP3 >> 6);
            if (srclkTmp == 0 &&_36_SRCLK == 1 ) //上升沿
            {
                _34_SER = (byte)((tempP3 >> 4) & 0x1);
                _c595_rigster >>= 1;
                _c595_rigster |= (byte)(_34_SER<<_c595_rigster_index);
                _c595_rigster_index--;
                if(_c595_rigster_index<0)_c595_rigster_index = 7;
            }
            else if (rclkTmp == 0&&_35_RCLK == 1)//上升沿 
            {
                _c595_register_bytes.Enqueue(_c595_rigster);
                P0_DotLed_Q.Enqueue(p0_data);
                _c595_rigster = 0;//模拟清零
            }
            srclkTmp = _36_SRCLK;
            rclkTmp = _35_RCLK;
        }
    }
    private void Read_P3_456_bit_data(byte addr,int bit,byte p0_data) 
    {
        if (isGnd_OE) 
        {
            switch (addr)
            {
                //ser
                case 0xB0 + 4:
                    {
                        // Debug.Log(addr.ToString("X"));
                        //Debug.Log("ser "+bit.ToString("X"));
                        _34_SER = (byte)bit;
                        break;
                    }
                //RCLK
                case 0xB0 + 5:
                    {
                        if (rclkTmp == 0 && bit == 1)//上升沿 
                        {
                            _c595_register_bytes.Enqueue(_c595_rigster);
                            P0_DotLed_Q.Enqueue(p0_data);
                            //Debug.Log("p0"+p0_data.ToString("X"));
                            //Debug.Log("rig "+_c595_rigster.ToString("X"));
                            _c595_rigster = 0;//模拟清零
                            _c595_rigster_index = 7;
                        }
                        rclkTmp = (byte)bit;
                        break;
                    }
                //SRCLK
                case 0xB0 + 6:
                    {
                        if (srclkTmp == 0 && bit == 1) //上升沿
                        {
                            if (_c595_rigster_index < 0) _c595_rigster_index = 7;
                            _c595_rigster |= (byte)(_34_SER << _c595_rigster_index);
                            //Debug.Log(((byte)(_34_SER << _c595_rigster_index)).ToString("X"));
                            //Debug.Log("ser_in_rg "+_c595_rigster.ToString("X"));
                            _c595_rigster_index--;
                        }
                        srclkTmp = (byte)bit;
                        break;
                    }
                default:
                    break;
            }
        }
    }
    private void GetOE_ON(bool is_C595_On)
    {
        isGnd_OE = is_C595_On;
    }
    #endregion

    #region 清理场景内存和运行开始时执行的方法
    private void Update() 
    {
        if (VM_Runtime.Instance.isRunning) 
        {
            Beer_exe.BeerPlay(P2_Beer_Q);
            led_exe.LED_Light_On(P2_Led_Q);
            dotLed_exe.DotLed_On(_c595_register_bytes,P0_DotLed_Q,isGnd_OE);
            uart_exe.InputChar_exe(InputTextChar);
        }
    }
    private void StartMono()
    {
        digitalTube_exe.digitalTubes_light();
        uart_exe.Output_exe(ReceiveChar);
    }
    private void ClearMono()
    {
        dotLed_exe.Close();
        led_exe.Close();
        Beer_exe.Close();
        digitalTube_exe.Close();
        oe.Close();
        P2_Led_Q?.Clear();
        P2_Beer_Q?.Clear();
        _c595_register_bytes?.Clear();
        P0_DotLed_Q?.Clear();
        InputTextChar?.Clear();
        ReceiveChar?.Clear();
        uart_exe.close();
        StopAllCoroutines();
    }
    #endregion
}

