using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE_ACC_IMM : InstructInfo
{
    public MOVE_ACC_IMM()
    {
        bytes = 2;
        cycles = 1;
        op0_mode = OpType.ACC;
        op1_mode = OpType.IMM8;
        opcode_name = "Move ACC,#imm8";
    }

    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort data = VM_8051_Mono.Instance.Read_Op(instr,1,mem_type.RAM);
        VM_8051_Mono.Instance.Write_Op(instr,data,0,mem_type.SFR);
    }
}
