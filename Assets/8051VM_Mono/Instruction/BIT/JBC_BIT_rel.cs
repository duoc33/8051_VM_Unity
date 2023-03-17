using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JBC_BIT_rel : InstructInfo
{
    public JBC_BIT_rel()
    {
        bytes = 3;
        cycles = 2;
        op0_mode = OpType.BIT;
        op1_mode = OpType.NONE;
        opcode_name = "JBC BIT, rel";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort op0 = VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.BIT);
        if (op0 != 0) {
            VM_8051_Mono.Instance.PC += (ushort)(sbyte)instr.op1;
            VM_8051_Mono.Instance.VmWrite(mem_type.BIT,instr.op0,0x0);
        }
    }
}
