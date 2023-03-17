using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE_Rn_ACC :InstructInfo
{
    public MOVE_Rn_ACC()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.Rn;
        op1_mode = OpType.ACC;
        opcode_name = "Move Rn,ACC";
    }

    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort data = VM_8051_Mono.Instance.Read_Op(instr,1,mem_type.SFR);
        VM_8051_Mono.Instance.Write_Op(instr,data,0,mem_type.RAM);
    }
}
