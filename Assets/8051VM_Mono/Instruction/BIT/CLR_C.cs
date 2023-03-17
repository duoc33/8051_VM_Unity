using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLR_C : InstructInfo
{
    public CLR_C()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.PSW_Cy;
        op1_mode = OpType.NONE;
        opcode_name = "ANL C";//bit  «Œªµÿ÷∑
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        VM_8051_Mono.Instance.Write_Op(instr,0,0, mem_type.BIT);
    }
}
