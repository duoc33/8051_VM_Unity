using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPL_C : InstructInfo
{
    public CPL_C()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.PSW_Cy;
        op1_mode = OpType.NONE;
        opcode_name = "CPL C";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        byte src = (byte)VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.BIT);
        VM_8051_Mono.Instance.Write_Op(instr, (ushort)(~src),0,mem_type.BIT);
    }
}
