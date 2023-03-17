using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MUL_AB : InstructInfo
{
    public MUL_AB() 
    { 
        bytes = 1;
        cycles = 4;
        op0_mode = OpType.ACC;
        op1_mode = OpType.B;
        opcode_name = "MUL AB";
    }
    public override void exec(_instruct instr)
    {
        ushort acc = VM_8051_Mono.Instance.Read_Op(instr,0,mem_type.RAM);
        ushort b = VM_8051_Mono.Instance.Read_Op(instr,1,mem_type.RAM);
        ushort result = (ushort)(acc * b);
        //set OV CY 标志位
        //OV 相乘 大于了 一个字节
        VM_8051_Mono.Instance.VmWrite(mem_type.BIT,0xD2, (byte)(result >255?1:0));
        //CY 总是要清零
        VM_8051_Mono.Instance.VmWrite(mem_type.BIT,0xD7,0);

        //高八位写在 第一个操作数(ACC)
        VM_8051_Mono.Instance.Write_Op(instr, (ushort)(result & 0xff), 0, mem_type.RAM);
        //低八位写在 第二个操作数(B)
        VM_8051_Mono.Instance.Write_Op(instr, (ushort)(result >> 8), 1,mem_type.RAM);
        base.exec(instr);
    }
}
