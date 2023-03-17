using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRL_ACC_AT_Ri : InstructInfo
{
    public XRL_ACC_AT_Ri()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.ACC;
        op1_mode = OpType.R0_R1;
        opcode_name = "XRL A,@Ri";
    }

    public override void exec(_instruct instr)
    {
        ushort src = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.RAM);
        ushort des = VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.RAM);
        VM_8051_Mono.Instance.Write_Op(instr, (ushort)(src ^ des), 0, mem_type.RAM);
        base.exec(instr);
    }
}
