using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Anger : SkillPhysic
{
    public Anger() : base("분노", "공격 받을수록 점점 더 격한 분노를 표출한다",
		20, false, SkillType.Physical) { }
}
