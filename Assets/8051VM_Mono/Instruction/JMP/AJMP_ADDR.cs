using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AJMP_ADDR : InstructInfo
{
    public AJMP_ADDR() 
    {
        bytes = 2;
        cycles = 2;
        op0_mode = OpType.NONE;
        op1_mode = OpType.NONE;
        opcode_name = "AJMP addr";
    }
    //将要跳转的地址 前八位放在 操作数0的位置，后高三位放在操作码的前三位
    //opcode: aaa0 0001   op0: xxxx xxxx  跳转地址:aaa xxxx xxxx 
    public override void exec(_instruct instr)
    {
        VM_8051_Mono.Instance.PC += 2;
        //0xf800 = 1111 1000 0000 0000 & PC =保留了PC的高5位，低11位置零
        //   instr.op0 | ((instr.opcode & 0xE0) << 3)  0xE0 = 1110 0000 保留高三位，并左移3位变成8~11位
        ushort temp = (ushort)(VM_8051_Mono.Instance.vm_pc() & 0xF800);
        ushort temp1 = (ushort)(((instr.opcode & 0xE0) << 3) | instr.op0);
        VM_8051_Mono.Instance.PC= (ushort)(temp | temp1);
        VM_8051_Mono.Instance.cycles += instr.info.cycles;
    }
}
