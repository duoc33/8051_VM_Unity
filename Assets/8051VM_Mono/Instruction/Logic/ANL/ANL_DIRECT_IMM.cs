using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANL_DIRECT_IMM : InstructInfo
{
    public ANL_DIRECT_IMM()
    {
        bytes = 3;
        cycles = 2;
        op0_mode = OpType.DIRECT;
        op1_mode = OpType.IMM8;
        opcode_name = "ANL DIRECT,ACC";
    }

    public override void exec(_instruct instr)
    {
        ushort src = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.RAM);
        ushort des = VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.RAM);
        VM_8051_Mono.Instance.Write_Op(instr, (ushort)(src & des), 0, mem_type.RAM);
        base.exec(instr);
    }
}
