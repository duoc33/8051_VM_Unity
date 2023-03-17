using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVC_ACC_At_ACC_DPTR : InstructInfo
{
    public MOVC_ACC_At_ACC_DPTR()
    {
        bytes = 1;
        cycles = 2;
        op0_mode = OpType.ACC;
        op1_mode = OpType.ACC_DPTR;
        opcode_name = "Movc ACC,@ACC+DPTR";
    }

    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort data = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.CODE);
        VM_8051_Mono.Instance.Write_Op(instr, data, 0, mem_type.CODE);
    }
}
