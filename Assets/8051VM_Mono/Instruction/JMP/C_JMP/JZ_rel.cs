using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JZ_rel : InstructInfo
{
    public JZ_rel() 
    {
        bytes = 2;
        cycles = 2;
        op0_mode = OpType.IMM8;
        op1_mode = OpType.NONE;
        opcode_name = "JZ rel";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort acc = VM_8051_Mono.Instance.VmRead(mem_type.SFR,0xE0);
        sbyte rel = (sbyte)instr.op0;
        VM_8051_Mono.Instance.PC += (ushort)((acc==0x0) ?rel:0);
    }
}
