using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPL_BIT : InstructInfo
{
    public CPL_BIT()
    {
        bytes = 2;
        cycles = 1;
        op0_mode = OpType.BIT;
        op1_mode = OpType.NONE;
        opcode_name = "CPL  bit";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        byte src = (byte)VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.BIT);
        VM_8051_Mono.Instance.VmWrite(mem_type.BIT, instr.op0, (byte)~src);
    }
}
