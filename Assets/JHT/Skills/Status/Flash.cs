using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Flash : SkillS
{
	public Flash() : base(
		"플래쉬",
		"눈이 부신 빛으로 상대의 명중률을 떨어뜨린다.",
		0,
		SkillType.Status,
		false,
		PokeType.Normal,
		20,
		70,
		true
		)
	{ }

	// 상대의 명중률이 1랭크 내려간다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
