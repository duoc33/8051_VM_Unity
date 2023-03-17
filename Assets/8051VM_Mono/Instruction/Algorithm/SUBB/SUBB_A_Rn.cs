using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUBB_A_Rn : SUBB_BASE
{
    public SUBB_A_Rn() : base(1, 1, OpType.ACC, OpType.Rn, "SUBB A,Rn") { }

    public override void exec(_instruct instr)
    {
        base.exec(instr);
    }
}
