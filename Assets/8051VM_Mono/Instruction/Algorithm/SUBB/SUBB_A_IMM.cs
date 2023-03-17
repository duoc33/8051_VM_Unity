using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUBB_A_IMM : SUBB_BASE
{
    public SUBB_A_IMM() : base(2, 1, OpType.ACC, OpType.IMM8, "SUBB A,#imm8") { }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
    }
}
