using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWAP_A : InstructInfo
{
    public SWAP_A()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.ACC;
        op1_mode = OpType.ACC;
        opcode_name = "SWAP, A";
    }

    public override void exec(_instruct instr)
    {
        ushort acc = VM_8051_Mono.Instance.Read_Op(instr,1,mem_type.SFR);
        acc = (ushort)((acc << 4) | (acc >> 4));
        VM_8051_Mono.Instance.Write_Op(instr,acc,0,mem_type.SFR);
        base.exec(instr);
    }
}
