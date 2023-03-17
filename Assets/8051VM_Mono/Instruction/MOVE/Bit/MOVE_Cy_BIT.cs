using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE_Cy_BIT : InstructInfo
{
    public MOVE_Cy_BIT()
    {
        bytes = 2;
        cycles = 1;
        op0_mode = OpType.PSW_Cy;
        op1_mode = OpType.BIT;
        opcode_name = "Move C,BIT";//R0,R1
    }

    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort data = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.CODE);
        VM_8051_Mono.Instance.Write_Op(instr, data, 0, mem_type.SFR);
    }
}
