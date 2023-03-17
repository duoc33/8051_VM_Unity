using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XCHD_A_AT_Ri : InstructInfo
{
    //交换低四位
    public XCHD_A_AT_Ri()
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.ACC;
        op1_mode = OpType.R0_R1;
        opcode_name = "XCHD A,@Ri";
    }

    public override void exec(_instruct instr)
    {
        ushort op0 = VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.RAM);
        ushort op1 = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.RAM);
        ushort temp = op0;
        op0 = (ushort)((op0 & ~0xf) | (op1 & 0xf));//op0的低四位清零 | op1的高四位清零 
        op1 = (ushort)((op1 & ~0xf) | (temp & 0xf));
        VM_8051_Mono.Instance.Write_Op(instr, op0,0,mem_type.RAM);
        VM_8051_Mono.Instance.Write_Op(instr, op1,1,mem_type.RAM);
        base.exec(instr);
    }
}
