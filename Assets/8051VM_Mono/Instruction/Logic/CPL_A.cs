using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPL_A : InstructInfo
{
    public CPL_A()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.ACC;
        op1_mode = OpType.NONE;
        opcode_name = "CPL A";
    }

    public override void exec(_instruct instr)
    {
        ushort data = VM_8051_Mono.Instance.Read_Op(instr,0,mem_type.RAM);
        VM_8051_Mono.Instance.Write_Op(instr, (ushort)~data, 0, mem_type.RAM);
        base.exec(instr);
    }
}
