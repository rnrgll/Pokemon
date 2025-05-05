using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Gust : SkillS
{
    public Gust() : base(
		"바람일으키기",
		"날개로 일으킨 격한 바람을 상대에게 부딪혀서 공격한다.",
		40,
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
