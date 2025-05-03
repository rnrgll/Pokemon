using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Peck : SkillS
{
	public Peck() : base(
		"쪼기",
		"날카롭고 뾰족한 부리나 뿔로 상대를 쪼아서 공격한다.",
		35,
		SkillType.Physical,
		false,
		PokeType.Flying,
		35,
		100
		) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
		}
	}
}
