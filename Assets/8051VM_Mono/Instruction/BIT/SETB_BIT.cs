using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SETB_BIT : InstructInfo
{
    public SETB_BIT()
    {
        bytes = 2;
        cycles = 1;
        op0_mode = OpType.BIT;
        op1_mode = OpType.NONE;
        opcode_name = "SETB  bit";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        VM_8051_Mono.Instance.VmWrite(mem_type.BIT, instr.op0, 0x1);
    }
}
