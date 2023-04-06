using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 高优先级打断低优先级，低优先级不能打断高优先级,同优先级根据中断顺序表
/// </summary>
public class VM_Interupt_System
{
    #region 中断需要的相应位地址
    //总允许位
    private const byte IE_EA = 0xA8 + 7;
    //INT0
    private const byte IE_EX0 = 0xA8 + 0;
    //T0
    private const byte IE_ET0 = 0xA8 + 1;
    //INT1
    private const byte IE_EX1 = 0xA8 + 2;
    //T1
    private const byte IE_ET1 = 0xA8 + 3;
    //UART串行通讯
    private const byte IE_ES = 0xA8 + 4;
    #endregion
    //外部中断0和1的触发
    const byte Int0_key3 = 0xB0 + 2;//key3
    const byte Int1_key4 = 0xB0 + 3;//key4
    byte key3;
    byte key4;
    //定时器0和1的触发
    const byte SFR_TCON_TF0 = 0x88 + 5;
    const byte SFR_TCON_TF1 = 0x88 + 7;
    //串行中断触发
    const byte SFR_SCON_RI = 0x98 + 0;
    const byte SFR_SCON_TI = 0x98 + 1;
    const byte IP = 0xB8;
    const byte IPH = 0xB7;
    bool Int0_Trigger = false;
    bool Timer0_Trigger = false;
    bool Int1_Trigger = false;
    bool Timer1_Trigger = false;
    bool Uart_Trigger = false;
    bool trigger = false;
    Queue<bool> triggers = new Queue<bool>();
    Queue<InteruptType> InteruptQueue = new Queue<InteruptType>();
    ushort currentInteruptPC;
    byte sp;
    public void Execute_Interupt() 
    {
        if (!InteruptEnable())return;
        //Debug.Log("EA"+InteruptEnable());
        if(VM_8051_Mono.Instance.in_interupt)return;
        if (InteruptQueue.Count > 0) 
        {
            InteruptQueue.Dequeue().handlerLogic();
            return;
        }
        TriggerJudge();
        //Debug.Log("in_interupt"+VM_8051_Mono.Instance.in_interupt);
        for (int i = 0; i < 5; i++)        //for循环对应了默认的优先顺序。
        {
            trigger = triggers.Dequeue();
            if(i>=2&&trigger==false)continue;
            if (InteruptEnter(i))
            {
                InteruptType _int = new InteruptType(i,enter_interupt_exe,trigger,key3,key4);
                InteruptQueue.Enqueue(_int);
                InteruptQueue.TrimExcess();
            }
        }
    }
    private void TriggerJudge() 
    {
        key3 = VM_8051_Mono.Instance.ReadSFR_BIT_ACTION(Int0_key3);
        Int0_Trigger = false;//VM_8051_Mono.Instance.isInputK3AloneKey | key3 == 0;
        triggers.Enqueue(Int0_Trigger);
        Timer0_Trigger = VM_8051_Mono.Instance.ReadSFR_BIT_ACTION(SFR_TCON_TF0) != 0 ? true : false;
        triggers.Enqueue(Timer0_Trigger);
        key4 = VM_8051_Mono.Instance.ReadSFR_BIT_ACTION(Int1_key4);
        Int1_Trigger = false;//VM_8051_Mono.Instance.isInputK4AloneKey | key4 == 0;
        triggers.Enqueue(Int1_Trigger);
        Timer1_Trigger = VM_8051_Mono.Instance.ReadSFR_BIT_ACTION(SFR_TCON_TF1) != 0 ? true : false;
        triggers.Enqueue(Timer1_Trigger);
        Uart_Trigger = (VM_8051_Mono.Instance.ReadSFR_BIT_ACTION(SFR_SCON_RI) | VM_8051_Mono.Instance.ReadSFR_BIT_ACTION(SFR_SCON_TI))!=0;
        triggers.Enqueue(Uart_Trigger);
    }
    private void enter_interupt_exe(ushort addr)
    {
        //进入中断
        VM_8051_Mono.Instance.in_interupt = true;

        //进入中段前存入PC值
        currentInteruptPC = VM_8051_Mono.Instance.PC;
        //得先把中断产生之前的地址压入栈中
        sp = VM_8051_Mono.Instance.Read_Sfr(0x81);
        //把PC低八位写入sp对应的内部ram区
        VM_8051_Mono.Instance.Write_Ram(++sp, (byte)(currentInteruptPC & 0xff));
        //把PC高八位写入sp对应的内部ram区
        VM_8051_Mono.Instance.Write_Ram(++sp, (byte)(currentInteruptPC >>8));
        //写入当前的sp的值进sp
        VM_8051_Mono.Instance.Write_Sfr(0x81, sp);
        VM_8051_Mono.Instance.PC = addr;
    }
    private bool InteruptEnable() 
    {
       return VM_8051_Mono.Instance.ReadSFR_BIT_ACTION(IE_EA)!=0;
    }
    private bool InteruptEnter(int interuptNum) 
    {
        switch (interuptNum)
        {
            case 0:
                return VM_8051_Mono.Instance.ReadSFR_BIT_ACTION(IE_EX0)!=0;
            case 1:
                return VM_8051_Mono.Instance.ReadSFR_BIT_ACTION(IE_ET0) != 0;
            case 2:
                return VM_8051_Mono.Instance.ReadSFR_BIT_ACTION(IE_EX1) != 0;
            case 3:
                return VM_8051_Mono.Instance.ReadSFR_BIT_ACTION(IE_ET1) != 0;
            case 4:
                return VM_8051_Mono.Instance.ReadSFR_BIT_ACTION(IE_ES) != 0;
            default:
                return false;
        }
    }
    
}
public class InteruptType 
{
    int NaturalProperty;
    private ushort startAddr;
    private Action<ushort> ExeAction;
    private byte mode1;
    private byte mode2;
    private byte Key3;
    private byte Key4;
    private bool Trigger;
    
    const byte SFR_TCON_IT0 = 0x88;
    const byte SFR_TCON_IT1 = 0x88 + 2;
    const byte SFR_TCON_IE0 = 0x88 + 1;
    const byte SFR_TCON_IE1 = 0x88 + 3;
    const byte SFR_TCON_TF0 = 0x88 + 5;
    const byte SFR_TCON_TF1 = 0x88 + 7;
    public InteruptType(int _pro,Action<ushort> action,bool trigger,byte key3,byte key4)
    {
        NaturalProperty = _pro;
        startAddr = (ushort)(0x0003 + 8*_pro);
        ExeAction += action;
        Trigger = trigger;
        Key3 = key3;
        Key4 = key4;
    }
    public void handlerLogic() 
    {
        switch (NaturalProperty)
        {
            case 0:
                INT0_EXE();
                break;
            case 1:
                Timer0_EXE();
                break;
            case 2:
                INT1_EXE();
                break;
            case 3:
                Timer1_EXE();
                break;
            case 4:
                UART_EXE();
                break;
            default:
                break;
        }
    }
    private void INT0_EXE() 
    {
        mode1 = VM_8051_Mono.Instance.ReadSFR_BIT_ACTION(SFR_TCON_IT0);//TCON_IT0
        if (mode1 == 1&&VM_8051_Mono.Instance.isInputK3AloneKey)//下降沿 
        {
            VM_8051_Mono.Instance.WriteSFR_BIT_ACTION(SFR_TCON_IE0, 1);//TCON_IE0
            ExeAction?.Invoke(startAddr);
        }
        else if(mode1 == 0&&Key3==0)
        {
            VM_8051_Mono.Instance.WriteSFR_BIT_ACTION(SFR_TCON_IE0, 1);
            ExeAction?.Invoke(startAddr);
        }
        else 
        {
            VM_8051_Mono.Instance.WriteSFR_BIT_ACTION(SFR_TCON_IE0, 0);
        }
    }
    private void INT1_EXE()
    {
        mode2 = VM_8051_Mono.Instance.ReadSFR_BIT_ACTION(SFR_TCON_IT1);//TCON_IT1
        if (mode2 == 1 && VM_8051_Mono.Instance.isInputK4AloneKey)//下降沿 
        {
            VM_8051_Mono.Instance.WriteSFR_BIT_ACTION(SFR_TCON_IE1, 1);//TCON_IE1
            ExeAction?.Invoke(startAddr);
        }
        else if (mode2 == 0 && Key4==0)
        {
            VM_8051_Mono.Instance.WriteSFR_BIT_ACTION(SFR_TCON_IE1, 1);
            ExeAction?.Invoke(startAddr);
        }
        else
        {
            VM_8051_Mono.Instance.WriteSFR_BIT_ACTION(SFR_TCON_IE1, 0);
        }
    }
    private void Timer0_EXE() 
    {
        if (Trigger)
        {
            ExeAction?.Invoke(startAddr);
            VM_8051_Mono.Instance.WriteSFR_BIT_ACTION(SFR_TCON_TF0,0);
        }
    }
    private void Timer1_EXE() 
    {
        if (Trigger) 
        {
            ExeAction?.Invoke(startAddr);
            VM_8051_Mono.Instance.WriteSFR_BIT_ACTION(SFR_TCON_TF1, 0);
        }
    }
    private void UART_EXE()
    {
        if (Trigger) 
        {
            ExeAction?.Invoke(startAddr);
        }
    }
}
