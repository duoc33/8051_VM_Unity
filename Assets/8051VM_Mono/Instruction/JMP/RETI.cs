using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RETI : InstructInfo
{
    public RETI()
    {
        bytes = 1;
        cycles = 2;
        op0_mode = OpType.NONE;
        op1_mode = OpType.NONE;
        opcode_name = "RETI";
    }
    public override void exec(_instruct instr)
    {
        ushort ret_addr;
        byte sp = VM_8051_Mono.Instance.Read_Sfr(0x81);
        ret_addr = (ushort)(VM_8051_Mono.Instance.Read_Ram(sp--) << 8);
        ret_addr |= VM_8051_Mono.Instance.Read_Ram(sp--);
        VM_8051_Mono.Instance.Write_Sfr(0x81, sp);
        VM_8051_Mono.Instance.PC = ret_addr;
        VM_8051_Mono.Instance.cycles += instr.info.cycles;
        VM_8051_Mono.Instance.in_interupt = false;
    }
}
