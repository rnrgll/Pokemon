using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Tackle : SkillS
{
	public Tackle() : base(
		"몸통박치기",
		"상대를 향해서 몸 전체를 부딪쳐가며 공격한다.",
		35,
		SkillType.Physical,
		false,
		PokeType.Normal,
		35,
		95
		) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
		}
	}
}
