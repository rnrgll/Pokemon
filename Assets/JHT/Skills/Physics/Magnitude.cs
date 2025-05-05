using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Magnitude : SkillS
{
	public Magnitude() : base(
		"매그니튜드",
		"땅을 흔들어서 주위에 있는 모두를 공격한다. 기술의 위력이 여러모로 바뀐다.",
		0,
		SkillType.Physical,
		false,
		PokeType.Ground,
		30,
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
