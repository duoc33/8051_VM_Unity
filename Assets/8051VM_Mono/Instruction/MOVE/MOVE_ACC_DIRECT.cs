using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE_ACC_DIRECT : InstructInfo
{
    public MOVE_ACC_DIRECT()
    {
        bytes = 2;
        cycles = 1;
        op0_mode = OpType.ACC;
        op1_mode = OpType.DIRECT;
        opcode_name = "Move ACC,DIRECT";
    }

    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort data = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.CODE);
        VM_8051_Mono.Instance.Write_Op(instr, data, 0, mem_type.SFR);
    }
}
