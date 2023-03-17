using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJNE_AT_Ri_IMM_rel : CJNE_BASE
{
    public CJNE_AT_Ri_IMM_rel() : base(3, 2, OpType.R0_R1, OpType.IMM8, "CJNE @Ri,IMM8 ,rel") { }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
    }
}
