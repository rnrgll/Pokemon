using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BodySlam : SkillS
{
	public BodySlam() : base(
		"몸통박치기",
		"몸전체를 이용해 들이받는다",
		35,
		SkillType.Physical,
		false,
		PokeType.Normal,
		35,
		95) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
		}
	}
}
