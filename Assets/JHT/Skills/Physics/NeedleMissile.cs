using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NeedleMissile : SkillPhysic
{
    public NeedleMissile() : base("바늘미사일", "뾰족한 침을 마구 발사하여 2~5회 연속으로 상대를 찌른다",
		14, false, SkillType.Physical) { }
}
