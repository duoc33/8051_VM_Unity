using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE_At_Ri_ACC : InstructInfo
{
    public MOVE_At_Ri_ACC()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.R0_R1;
        op1_mode = OpType.ACC;
        opcode_name = "Move @Ri,ACC";//R0,R1
    }
    
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort data = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.CODE);
        VM_8051_Mono.Instance.Write_Op(instr, data, 0, mem_type.SFR);
    }
}
