using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SETB_C : InstructInfo
{
    public SETB_C()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.PSW_Cy;
        op1_mode = OpType.NONE;
        opcode_name = "SETB C";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        VM_8051_Mono.Instance.Write_Op(instr, 0x1, 0, mem_type.BIT);
    }
}
