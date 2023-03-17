using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUBB_BASE : InstructInfo
{
    public SUBB_BASE(ushort Bytes, int Cycles, OpType op0, OpType op1, string Opcode_name) : base()
    {
        bytes = Bytes;
        cycles = Cycles;
        op0_mode = op0;
        op1_mode = op1;
        opcode_name = Opcode_name;
    }

    public override void exec(_instruct instr)
    {
        ushort src = VM_8051_Mono.Instance.Read_Op(instr, 1, mem_type.RAM);
        src += VM_8051_Mono.Instance.VmRead(mem_type.BIT,0xD7);//加上CY标志位的值
        ushort acc = VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.RAM);

        ushort result = (ushort)(acc-src);
        VM_8051_Mono.Instance.Write_Op(instr, result, 0, mem_type.RAM);

        //处理其他标志位CY OV AC 向前借位
        //如果acc不够减，则CY置1
        VM_8051_Mono.Instance.VmWrite(mem_type.BIT, 0xD7, (byte)((acc < src) ? 1 : 0));
        //如果ACC低四位 不够减src低四位，则AC置1
        if ((acc & 0x0F) < (src & 0x0F))
        {
            VM_8051_Mono.Instance.VmWrite(mem_type.BIT, 0xD6, 0x1);
        }
        else
        {
            VM_8051_Mono.Instance.VmWrite(mem_type.BIT, 0xD6, 0x0);
        }
        //OV 如果 第七位不够减，acc-src<0 或者，第六位以下的不够减 两个只有一个满足时，置1
        int bit_7 = (acc < src) ? 1 : 0;
        int bit_6 = (acc & 0x7f) < (src & 0x7f)?1:0;
        VM_8051_Mono.Instance.VmWrite(mem_type.BIT, 0xD2, (byte)(bit_6 ^ bit_7));
        base.exec(instr);
    }
}
