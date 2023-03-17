using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ORL_C_NOT_BIT : InstructInfo
{
    public ORL_C_NOT_BIT()
    {
        bytes = 2;
        cycles = 2;
        op0_mode = OpType.PSW_Cy;
        op1_mode = OpType.BIT;
        opcode_name = "ORL C , ~bit";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort src_bit = VM_8051_Mono.Instance.VmRead(mem_type.BIT, instr.op0);
        byte dest = (byte)VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.BIT);
        VM_8051_Mono.Instance.Write_Op(instr, (ushort)((~src_bit) | dest), 0, mem_type.BIT);
    }
}
