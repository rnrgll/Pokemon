using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Earthquake : SkillS
{
    public Earthquake() : base(
		"지진",
		"강한 지진을 일으켜 주변 땅에 있는 것들에 피해를 준다",
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
