using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUBB_A_AT_Ri : SUBB_BASE
{
    public SUBB_A_AT_Ri():base(1,1,OpType.ACC,OpType.R0_R1,"SUBB A,@Ri") { }
    public override void exec(_instruct instr)
    {
        base.exec(instr);
    }
}
