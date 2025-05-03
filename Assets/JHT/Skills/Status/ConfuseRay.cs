using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ConfuseRay : SkillS
{
	public ConfuseRay() : base(
		"이상한빛",
		"이상한 빛을 상대에게 비춰 혼란시킨다. 상대를 혼란 상태로 만든다.",
		0,
		SkillType.Status,
		false,
		PokeType.Ghost,
		10,
		100
		)
	{ }

	// 상대를 혼란 상태로 만든다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
