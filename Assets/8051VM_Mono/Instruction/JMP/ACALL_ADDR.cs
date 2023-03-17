using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACALL_ADDR : InstructInfo
{   
    public ACALL_ADDR() 
    {
        bytes = 2;
        cycles = 2;
        op0_mode = OpType.NONE;
        op1_mode = OpType.NONE;
        opcode_name = "ACALL addr11";
    }
    public override void exec(_instruct instr)
    {
        VM_8051_Mono.Instance.PC += instr.info.bytes;

        //取sp的值,里面是一个内部RAM地址并加1
        byte sp = (byte)(VM_8051_Mono.Instance.VmRead(mem_type.SFR, 0x81)+1);
        //取PC的低八位，写进sp里取得的sp+1的地址里
        VM_8051_Mono.Instance.VmWrite(mem_type.RAM, sp++, (byte)(VM_8051_Mono.Instance.PC &0xFF));
        ///取PC的高八位，写进sp里取得的sp+2的地址里
        VM_8051_Mono.Instance.VmWrite(mem_type.RAM, sp, (byte)(VM_8051_Mono.Instance.PC>>8));
        //将sp值写回sp
        VM_8051_Mono.Instance.VmWrite(mem_type.SFR, 0x81, sp);

        //PC的值变化 1111 1000 0000 0000  E0=1110 0000
        ushort temp = (ushort)(VM_8051_Mono.Instance.PC & 0xF800);
        ushort temp1 = (ushort)(((instr.opcode & 0xE0) << 3) | instr.op0);
        VM_8051_Mono.Instance.PC = (ushort)(temp | temp1);

        VM_8051_Mono.Instance.cycles += instr.info.cycles;
    }
}
