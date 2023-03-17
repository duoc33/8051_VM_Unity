using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DA_A : InstructInfo
{
    public DA_A() 
    {
        bytes = 1;
        cycles = 1;
        op0_mode = OpType.ACC;
        op1_mode = OpType.ACC;
        opcode_name = "DA A";
    }
    public override void exec(_instruct instr)
    {
        //根据手册上的要求
        ushort acc = VM_8051_Mono.Instance.VmRead(mem_type.SFR,0xE0);
        byte ac = VM_8051_Mono.Instance.VmRead(mem_type.BIT,0xD6);
        byte cy = VM_8051_Mono.Instance.VmRead(mem_type.BIT, 0xD7);

        if (((acc & 0xf) > 9) || (ac == 0x1)) 
        {
            acc += 6;
        }
        //如果进位了
        if (acc >= 0x100) 
        {
            cy = 1;
        }
        if (((acc&0xf0)>0x90)||(cy==0x1))
        {
            acc += 0x60;
        }
        if (acc >= 0x100)
        {
            cy = 1;
        }
        VM_8051_Mono.Instance.VmWrite(mem_type.SFR,0xE0, (byte)acc);
        VM_8051_Mono.Instance.VmWrite(mem_type.BIT,0xD7,cy);

        base.exec(instr);
    }
}
