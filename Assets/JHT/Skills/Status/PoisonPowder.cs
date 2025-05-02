using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PoisonPowder : SkillStatus
{
	public PoisonPowder() : base("독가루", "유독한 가루를 뿌려 중독시킨다",
		0, false, SkillType.Status,PokeType.Poison,35,74.61f) { }
}
