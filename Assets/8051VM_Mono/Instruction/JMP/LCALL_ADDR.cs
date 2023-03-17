using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCALL_ADDR : InstructInfo
{
    public LCALL_ADDR()
    {
        bytes = 3;
        cycles = 2;
        op0_mode = OpType.NONE;
        op1_mode = OpType.NONE;
        opcode_name = "LCALL addr11";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        byte sp = (byte)(VM_8051_Mono.Instance.Read_Sfr(0x81) + 1);
        VM_8051_Mono.Instance.VmWrite(mem_type.RAM, sp++, (byte)(VM_8051_Mono.Instance.PC & 0xFF));//µÍ°ËÎ»
        VM_8051_Mono.Instance.VmWrite(mem_type.RAM, sp, (byte)(VM_8051_Mono.Instance.PC >> 8));//¸ß°ËÎ»
        VM_8051_Mono.Instance.VmWrite(mem_type.SFR, 0x81, sp);
        VM_8051_Mono.Instance.PC = (ushort)(instr.op0 << 8 | instr.op1);

    }
}
