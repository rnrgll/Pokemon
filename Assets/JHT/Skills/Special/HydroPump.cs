using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class HydroPump : SkillS
{
    public HydroPump() : base(
		"하이드로펌프",
		"대량의 물을 세찬 기세로 상대에게 발사하여 공격한다.", 
		120,
		SkillType.Special,
		false,
		PokeType.Water,
		5,
		80

		) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
		}
	}
}
