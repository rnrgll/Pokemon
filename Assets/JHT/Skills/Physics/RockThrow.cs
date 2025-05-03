using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class RockThrow : SkillS
{
	public RockThrow() : base(
		"돌떨구기",
		"작은 바위를 들어올려 상대에게 내던져서 공격한다.",
		50,
		SkillType.Physical,
		false,
		PokeType.Rock,
		15,
		90
		) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
		}
	}
}
