using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RET : InstructInfo
{
    public RET()
    {
        bytes = 1;
        cycles = 2;
        op0_mode = OpType.NONE;
        op1_mode = OpType.NONE;
        opcode_name = "RET";
    }
    public override void exec(_instruct instr)
    {
        ushort ret_addr;
        byte sp = VM_8051_Mono.Instance.Read_Sfr(0x81);
        //第一次读取的sp的值 得到的返回的地址值为高八位
        ret_addr = (ushort)(VM_8051_Mono.Instance.Read_Ram(sp--)<<8);
        //第一次读取的sp的值 得到的返回的地址值为低八位，并将两个值加起来，得到返回地址
        ret_addr |= VM_8051_Mono.Instance.Read_Ram(sp--);
        //sp值写回去
        VM_8051_Mono.Instance.Write_Sfr(0x81, sp);
        //更改PC值
        VM_8051_Mono.Instance.PC = ret_addr;
        VM_8051_Mono.Instance.cycles+= instr.info.cycles;
    }
}
