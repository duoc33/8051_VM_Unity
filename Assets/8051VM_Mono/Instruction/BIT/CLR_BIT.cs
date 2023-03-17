using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLR_BIT : InstructInfo
{
    public CLR_BIT()
    {
        bytes = 2;
        cycles = 1;
        op0_mode = OpType.BIT;
        op1_mode = OpType.NONE;
        opcode_name = "CLR  bit";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        VM_8051_Mono.Instance.VmWrite(mem_type.BIT, instr.op0,0x0);
    }
}
