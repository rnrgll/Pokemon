using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Scratching : SkillPhysic
{
    public Scratching() : base("마구할퀴기", "뾰족하면서 날카로운 손톱이나 발톱으로 2~5회 연속으로 난도질한다",
		18, false, SkillType.Physical) { }
}
