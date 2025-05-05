using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class DrillPeck : SkillS
{
	public DrillPeck() : base(
		"회전부리",
		"회전하면서 뾰족한 부리를 상대에게 꿰찔러 공격한다.",
		80,
		SkillType.Physical,
		false,
		PokeType.Flying,
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
