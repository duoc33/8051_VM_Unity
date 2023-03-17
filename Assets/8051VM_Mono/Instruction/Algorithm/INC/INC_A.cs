using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INC_A : InstructInfo
{
    public INC_A()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.ACC;
        op1_mode = OpType.ACC;
        opcode_name = "INC A";
    }

    public override void exec(_instruct instr)
    {
        ushort src = VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.RAM);
        VM_8051_Mono.Instance.Write_Op(instr, (ushort)(src + 1), 0, mem_type.RAM);
        base.exec(instr);
    }
}
