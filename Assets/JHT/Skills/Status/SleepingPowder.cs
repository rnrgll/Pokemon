using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SleepingPowder : SkillStatus
{
    public SleepingPowder() : base("수면가루", "졸음을 유발하는 가루를 뿌려 재운다",
		0, false, SkillType.Status) { }
}
