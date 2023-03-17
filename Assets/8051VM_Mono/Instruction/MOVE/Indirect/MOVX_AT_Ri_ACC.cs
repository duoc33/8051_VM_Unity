using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVX_AT_Ri_ACC : InstructInfo
{
    public MOVX_AT_Ri_ACC()
    {
        bytes = 1;
        cycles = 2;
        op0_mode = OpType.R0_R1;
        op1_mode = OpType.ACC;
        opcode_name = "Movx @Ri,ACC";
    }

    public override void exec(_instruct instr)
    {
        ushort data = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.Exteranl);
        VM_8051_Mono.Instance.Write_Op(instr, data, 0, mem_type.Exteranl);
        base.exec(instr);
    }
}
