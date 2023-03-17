using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLR_A : InstructInfo
{
    public CLR_A()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.ACC;
        op1_mode = OpType.NONE;
        opcode_name = "CLR A";
    }

    public override void exec(_instruct instr)
    {
        VM_8051_Mono.Instance.Write_Op(instr,0,0,mem_type.RAM);
        base.exec(instr);
    }
}
