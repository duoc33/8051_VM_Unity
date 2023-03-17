using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE_DIRECT_DIRECT : InstructInfo
{
    public MOVE_DIRECT_DIRECT()
    {
        bytes = 3;
        cycles = 2;
        op0_mode = OpType.DIRECT;
        op1_mode = OpType.DIRECT;
        opcode_name = "Move DIRECT,DIRECT";
    }

    //Move direct ,direct 中的des 和 src是相反的 
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        ushort data = VM_8051_Mono.Instance.Read_Op(instr, 0, mem_type.CODE);
        VM_8051_Mono.Instance.Write_Op(instr, data, 1, mem_type.CODE);
    }
}
