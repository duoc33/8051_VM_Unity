using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE_Rn_IMM : InstructInfo
{
    public MOVE_Rn_IMM()
    {
        bytes = 2;
        cycles = 1;
        op0_mode = OpType.Rn;
        op1_mode = OpType.IMM8;
        opcode_name = "Move Rn,#imm8";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        //byte RregAddr =(byte)(instr.opcode & 0x07);//0x07 => 0000 0111 得到操作码低三位的地址 
        //VM_8051_Mono.Instance.VmWrite(mem_type.RAM, RregAddr, instr.op0);

        //读取op1的数据 src
        ushort data = VM_8051_Mono.Instance.Read_Op(instr,1,mem_type.RAM);
        //把数据 写入到des ，如果是Rn 则在opcode中的第三位找
        VM_8051_Mono.Instance.Write_Op(instr, data,0, mem_type.RAM);
    }
}
