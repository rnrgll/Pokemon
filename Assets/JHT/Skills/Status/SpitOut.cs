using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SpitOut : SkillStatus
{
    public SpitOut() : base("실뿜기", "입에서 실을 뿜어서 적에게 얽혀 속도를 떨어트린다",
		0, false, SkillType.Status) { }
}
