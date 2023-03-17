using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE_ACC_Rn : InstructInfo
{
    public MOVE_ACC_Rn()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.ACC;
        op1_mode = OpType.Rn;
        opcode_name = "Move ACC,Rn";
    }

    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort data = VM_8051_Mono.Instance.Read_Op(instr,1,mem_type.RAM);
        VM_8051_Mono.Instance.Write_Op(instr,data,0,mem_type.SFR);
    }
}
