using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUSH_DIRECT : InstructInfo
{
    public PUSH_DIRECT()
    {
        bytes = 2;
        cycles = 2;
        op0_mode = OpType.AT_SP;
        op1_mode = OpType.DIRECT;
        opcode_name = "PUSH DIRECT";//R0,R1
    }

    public override void exec(_instruct instr)
    {
        VM_8051_Mono.Instance.VmWrite(mem_type.SFR, 0x81, (byte)(VM_8051_Mono.Instance.VmRead(mem_type.SFR,0x81)+1));
        ushort data = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.RAM);
        VM_8051_Mono.Instance.Write_Op(instr, data, 0, mem_type.RAM);
        base.exec(instr);
    }
}
