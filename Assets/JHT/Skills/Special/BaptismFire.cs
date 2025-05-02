using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BaptismFire : SkillSpecial
{
    public BaptismFire() : base("불꽃세례", "작은 불꽃을 날린다. 상대에게 때때로 화상을 입힌다 ",
		40, false, SkillType.Special) { }
}
