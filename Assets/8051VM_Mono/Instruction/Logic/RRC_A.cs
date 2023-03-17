using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RRC_A : InstructInfo
{
    public RRC_A()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.ACC;
        op1_mode = OpType.NONE;
        opcode_name = "RRC A";
    }

    public override void exec(_instruct instr)
    {
        ushort data = VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.RAM);

        byte c = VM_8051_Mono.Instance.VmRead(mem_type.BIT, 0xD7);
        byte A_ML = (byte)(data &0x1);//×îµÍÎ»
        VM_8051_Mono.Instance.VmWrite(mem_type.BIT, 0xD7, A_ML);
        data = (ushort)((c>0 ? 0x80 :0x0) | (data >> 1)) ;
        VM_8051_Mono.Instance.Write_Op(instr, data, 0, mem_type.RAM);

        base.exec(instr);
    }
}
