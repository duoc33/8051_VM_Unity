using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOP : InstructInfo
{
    public NOP() 
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.NONE;
        op1_mode = OpType.NONE;
        opcode_name = "NOP";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
    }
}
