using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEC_Rn : InstructInfo
{
    public DEC_Rn()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.Rn;
        op1_mode = OpType.Rn;
        opcode_name = "DEC Rn";
    }

    public override void exec(_instruct instr)
    {
        ushort src = VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.RAM);
        VM_8051_Mono.Instance.Write_Op(instr, (ushort)(src - 1), 0, mem_type.RAM);
        base.exec(instr);
    }
}
