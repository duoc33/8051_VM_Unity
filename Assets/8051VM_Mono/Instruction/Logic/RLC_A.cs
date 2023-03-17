using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLC_A : InstructInfo
{
    public RLC_A()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.ACC;
        op1_mode = OpType.NONE;
        opcode_name = "RLC A";
    }

    public override void exec(_instruct instr)
    {
        ushort data = VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.RAM);
        byte c = VM_8051_Mono.Instance.VmRead(mem_type.BIT, 0xD7);//CY的位 给到A的第一位
        byte A_MH = (byte)(data >> 7);//A 的最高位给到CY
        VM_8051_Mono.Instance.VmWrite(mem_type.BIT, 0xD7, A_MH);//更改CY位的值位A的最高位
        data = (ushort)(c | (data << 1));//A的值为 左移1位，并把CY位的原始值给到第一位
        VM_8051_Mono.Instance.Write_Op(instr,data,0,mem_type.RAM);
        base.exec(instr);
    }
}
