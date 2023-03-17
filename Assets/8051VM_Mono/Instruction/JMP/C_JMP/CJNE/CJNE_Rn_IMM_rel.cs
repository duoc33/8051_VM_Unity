using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJNE_Rn_IMM_rel : CJNE_BASE
{
    public CJNE_Rn_IMM_rel() : base(3, 2, OpType.Rn, OpType.IMM8, "CJNE Rn,IMM8 ,rel") { }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
    }
}
