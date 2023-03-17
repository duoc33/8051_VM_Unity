using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MOVE_DIRECT_IMM : InstructInfo
{
    public MOVE_DIRECT_IMM()
    {
        bytes = 3;
        cycles = 2;
        op0_mode = OpType.DIRECT;
        op1_mode = OpType.IMM8;
        opcode_name = "Move DIRECT,#imm8";
    }

    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort data = VM_8051_Mono.Instance.Read_Op(instr,1,mem_type.CODE);
        VM_8051_Mono.Instance.Write_Op(instr,data,0,mem_type.RAM);
    }
}

