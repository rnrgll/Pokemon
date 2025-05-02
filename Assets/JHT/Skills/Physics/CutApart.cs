using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CutApart : SkillPhysic
{
    public CutApart() : base("베어가르기", "예리한 발톱이나 칼날로 벤다. 급소에 맞히기 쉽다",
		70, false, SkillType.Physical) { }
}
