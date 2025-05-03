using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MirrorMove : SkillS
{
	public MirrorMove() : base(
		"따라하기",
		"상대가 사용한 기술을 흉내 내어 자신도 똑같은 기술을 쓴다.",
		0,
		SkillType.Status,
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
