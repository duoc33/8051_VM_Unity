using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJNE_BASE : InstructInfo
{
    public CJNE_BASE(ushort B,int C,OpType op0,OpType op1,string Opcode_name) :base()
    {
        bytes = B;
        cycles = C;
        op0_mode = op0;
        op1_mode = op1;
        opcode_name = Opcode_name;
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort op0_data = VM_8051_Mono.Instance.Read_Op(instr,0,mem_type.RAM);
        ushort op1_data;
        if (instr.info.op1_mode==OpType.DIRECT) {
            op1_data = VM_8051_Mono.Instance.Read_Ram(instr.op0);
        }
        else
        {
            op1_data = instr.op0;
        }
        
        if (op0_data != op1_data) {
            VM_8051_Mono.Instance.PC +=(ushort)(sbyte)instr.op1;
        }
        if (op0_data < op1_data) {
            VM_8051_Mono.Instance.VmWrite(mem_type.BIT,0xD7,1);//CY±êÖ¾Î»ÖÃ1
        }
        else
        {
            VM_8051_Mono.Instance.VmWrite(mem_type.BIT, 0xD7, 0);
        }

    }
}
