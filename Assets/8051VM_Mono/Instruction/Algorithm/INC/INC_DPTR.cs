using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INC_DPTR : InstructInfo
{
    public INC_DPTR()
    {
        bytes = 1;
        cycles = 2;
        op0_mode = OpType.DPTR;
        op1_mode = OpType.DPTR;
        opcode_name = "INC DPTR";
    }

    public override void exec(_instruct instr)
    {
        ushort src = VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.RAM);
        VM_8051_Mono.Instance.Write_Op(instr, (ushort)(src + 1), 0, mem_type.RAM);
        base.exec(instr);
    }
}
