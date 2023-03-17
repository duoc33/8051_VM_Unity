using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVC_ACC_At_ACC_PC : InstructInfo
{
    public MOVC_ACC_At_ACC_PC()
    {
        bytes = 1;
        cycles = 2;
        op0_mode = OpType.ACC;
        op1_mode = OpType.ACC_PC;
        opcode_name = "Movc ACC,@ACC+PC";
    }

    public override void exec(_instruct instr)
    {
        ushort data = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.CODE);
        VM_8051_Mono.Instance.Write_Op(instr, data, 0, mem_type.CODE);
        base.exec(instr);
    }
}
