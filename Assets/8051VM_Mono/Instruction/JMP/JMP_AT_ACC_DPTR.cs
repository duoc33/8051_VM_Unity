using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JMP_AT_ACC_DPTR : InstructInfo
{
    public JMP_AT_ACC_DPTR() 
    {
        bytes = 1;
        cycles = 2;
        op0_mode = OpType.NONE;
        op1_mode = OpType.NONE;
        opcode_name = "JMP_AT_ACC_DPTR";
    }
    public override void exec(_instruct instr)
    {
        ushort acc = VM_8051_Mono.Instance.Read_Sfr(0xE0);
        ushort dptr = (ushort)(VM_8051_Mono.Instance.Read_Sfr(0x83) << 8 | VM_8051_Mono.Instance.Read_Sfr(0x82));

        VM_8051_Mono.Instance.PC = (ushort)(acc + dptr);
        VM_8051_Mono.Instance.cycles +=instr.info.cycles;
    }
}
