using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJNE_ACC_DIRECT_rel : CJNE_BASE
{
    public CJNE_ACC_DIRECT_rel() : base(3, 2, OpType.ACC, OpType.DIRECT, "CJNE A ,DIRECT ,rel") { }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
    }
}
