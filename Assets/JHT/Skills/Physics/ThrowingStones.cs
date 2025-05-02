using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ThrowingStones : SkillPhysic
{
	public ThrowingStones() : base("돌떨구기", "작은 바위를 던져 공격한다",
		50, false, SkillType.Physical,PokeType.Rock,15,89.45f) { }
}
