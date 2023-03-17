using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJNE_ACC_IMM_rel : CJNE_BASE
{
    public CJNE_ACC_IMM_rel() : base(3, 2, OpType.ACC, OpType.IMM8, "CJNE A ,IMM8 ,rel") { }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
    }
}
