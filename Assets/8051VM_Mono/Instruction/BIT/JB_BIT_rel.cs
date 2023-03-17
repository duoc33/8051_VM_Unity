using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JB_BIT_rel : InstructInfo
{
    public JB_BIT_rel()
    {
        bytes = 3;
        cycles = 2;
        op0_mode = OpType.BIT;
        op1_mode = OpType.NONE;
        opcode_name = "JB BIT, rel";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort op0 = VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.BIT);
        VM_8051_Mono.Instance.PC += (ushort)((op0 != 0) ? (sbyte)instr.op1 : 0);
    }
}
