using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JC_rel : InstructInfo
{
    public JC_rel()
    {
        bytes = 2;
        cycles = 2;
        op0_mode = OpType.PSW_Cy;
        op1_mode = OpType.NONE;
        opcode_name = "JC rel";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort src_cy = VM_8051_Mono.Instance.Read_Op(instr,0,mem_type.BIT);
        VM_8051_Mono.Instance.PC += (ushort)((src_cy != 0) ? (sbyte)instr.op0 : 0);
    }
}
