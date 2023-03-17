using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJMP_ADDR : InstructInfo
{
    public LJMP_ADDR()
    {
        bytes = 3;
        cycles = 2;
        op0_mode = OpType.NONE;
        op1_mode = OpType.NONE;
        opcode_name = "LJMP addr16";
    }
    //将要跳转的地址 前八位放在 操作数0的位置，后高三位放在操作码的前三位
    //opcode: aaa0 0001   op0: xxxx xxxx  跳转地址:aaa xxxx xxxx 
    public override void exec(_instruct instr)
    {
        VM_8051_Mono.Instance.PC = (ushort)(instr.op0<<8 | instr.op1);
        VM_8051_Mono.Instance.cycles += instr.info.cycles;
    }
}
