using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XCH_A_AT_Ri : InstructInfo
{
    public XCH_A_AT_Ri()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.ACC;
        op1_mode = OpType.R0_R1;
        opcode_name = "XCH A,@Ri";
    }

    public override void exec(_instruct instr)
    {
        ushort op0 = VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.RAM);
        ushort op1 = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.RAM);
        VM_8051_Mono.Instance.Write_Op(instr, op0, 1, mem_type.RAM);
        VM_8051_Mono.Instance.Write_Op(instr, op1, 0, mem_type.RAM);
        base.exec(instr);
    }
}
