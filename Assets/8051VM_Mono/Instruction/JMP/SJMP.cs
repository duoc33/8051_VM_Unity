using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJMP : InstructInfo
{
    public SJMP() 
    {
        bytes = 2;
        cycles =2;
        op0_mode=OpType.NONE;
        op1_mode=OpType.NONE;
        opcode_name = "SJMP rel";
    }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
        //SJMP的操作，就是把当前PC值 加上操作数0(op0)的值,得到PC跳转的位置
        VM_8051_Mono.Instance.PC += (ushort)(sbyte)instr.op0;//这是一个有符号数，需要把byte转换以下
    }
}
