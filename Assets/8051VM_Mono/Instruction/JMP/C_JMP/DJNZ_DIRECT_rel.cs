using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DJNZ_DIRECT_rel : InstructInfo
{
    public DJNZ_DIRECT_rel()
    {
        bytes = 3;
        cycles = 2;
        op0_mode = OpType.DIRECT;
        op1_mode = OpType.IMM8;
        opcode_name = "JNZ rel";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort direct = (ushort)(VM_8051_Mono.Instance.Read_Op(instr,0,mem_type.RAM)-1);
        VM_8051_Mono.Instance.Write_Op(instr, direct, 0,mem_type.RAM);
        VM_8051_Mono.Instance.PC +=(ushort) ((direct != 0)?(sbyte)instr.op1:0);
    }
}
