using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE_DPTR_IMM16 : InstructInfo
{
    public MOVE_DPTR_IMM16()
    {
        bytes = 3;
        cycles = 2;
        op0_mode = OpType.DPTR;
        op1_mode = OpType.IMM16;
        opcode_name = "Move DPTR,IMM15-8,IMM8-0";
    }

    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort data = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.CODE);
        VM_8051_Mono.Instance.Write_Op(instr, data, 0, mem_type.SFR);
    }
}
