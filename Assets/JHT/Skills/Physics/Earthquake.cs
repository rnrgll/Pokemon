using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Earthquake : SkillS
{
    public Earthquake() : base(
		"지진",
		"지진의 충격으로 주위에 있는 모든 것을 공격한다.",
		100,
		SkillType.Physical,
		false,
		PokeType.Ground,
		10,
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
