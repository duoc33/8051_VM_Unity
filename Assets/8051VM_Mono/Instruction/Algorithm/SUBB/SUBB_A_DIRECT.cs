using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUBB_A_DIRECT : SUBB_BASE
{
    public SUBB_A_DIRECT() : base(2, 1, OpType.ACC, OpType.DIRECT, "SUBB A,DIRECT") { }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
    }
}
