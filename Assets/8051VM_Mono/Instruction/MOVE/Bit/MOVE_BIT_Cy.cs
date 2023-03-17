using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE_BIT_Cy : InstructInfo
{
    public MOVE_BIT_Cy()
    {
        bytes = 2;
        cycles = 2;
        op0_mode = OpType.BIT;
        op1_mode = OpType.PSW_Cy;
        opcode_name = "Move BIT,C";//R0,R1
    }

    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort data = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.CODE);
        VM_8051_Mono.Instance.Write_Op(instr, data, 0, mem_type.SFR);
    }
}
