using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ORL_DIRECT_ACC : InstructInfo
{
    public ORL_DIRECT_ACC()
    {
        bytes = 2;
        cycles = 1;
        op0_mode = OpType.DIRECT;
        op1_mode = OpType.ACC;
        opcode_name = "ORL DIRECT,ACC";
    }

    public override void exec(_instruct instr)
    {
        ushort src = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.RAM);
        ushort des = VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.RAM);
        VM_8051_Mono.Instance.Write_Op(instr, (ushort)(src | des), 0, mem_type.RAM);
        base.exec(instr);
    }
}
