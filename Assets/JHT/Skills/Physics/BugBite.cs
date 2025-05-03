using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BugBite : SkillS
{
	public BugBite() : base(
		"벌레먹음",
		"물어서 공격한다. 상대가 나무열매를 지니고 있을 때 먹어서 나무열매의 효과를 받을 수 있다.",
		60,
		SkillType.Physical,
		false,
		PokeType.Bug,
		20,
		100
		)
	{ }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
		}
	}
}
