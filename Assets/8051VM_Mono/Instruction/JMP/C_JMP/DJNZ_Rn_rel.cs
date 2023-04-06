using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DJNZ_Rn_rel : InstructInfo
{
    public DJNZ_Rn_rel()
    {
        bytes = 2;
        cycles = 2;
        op0_mode = OpType.Rn;
        op1_mode = OpType.IMM8;
        opcode_name = "DJNZ_Rn_rel";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort rn = (ushort)(VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.RAM)-1);
        VM_8051_Mono.Instance.Write_Op(instr, rn, 0, mem_type.RAM);
        if (rn != 0) 
        {
            VM_8051_Mono.Instance.PC = (ushort)(VM_8051_Mono.Instance.PC + (sbyte)instr.op0);
        }
    }
}
