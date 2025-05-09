using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SuperFang : SkillS
{
	public SuperFang() : base(
		"분노의앞니",
		"날카로운 앞니로 강하게 물어서 공격한다. 상대의 HP는 절반이 된다.",
		0,
		SkillType.Physical,
		false,
		PokeType.Normal,
		10,
		90
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
