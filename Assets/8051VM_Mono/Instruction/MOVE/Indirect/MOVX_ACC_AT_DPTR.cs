using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVX_ACC_AT_DPTR : InstructInfo
{
    public MOVX_ACC_AT_DPTR()
    {
        bytes = 1;
        cycles = 2;
        op0_mode = OpType.ACC;
        op1_mode = OpType.AT_DPTR;
        opcode_name = "Movx ACC,@DPTR";
    }

    public override void exec(_instruct instr)
    {
        ushort data = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.Exteranl);
        VM_8051_Mono.Instance.Write_Op(instr, data, 0, mem_type.Exteranl);
        base.exec(instr);
    }
}
