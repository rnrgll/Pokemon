using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Growth : SkillS
{
    public Growth() : base(
		"성장",
		"몸을 일시에 크게 성장시켜 자신의 공격과 특수공격을 올린다.",
		0,
		SkillType.Status,
		true,
		PokeType.Normal,
		40,
		100
		) { }

	// 자신의 특수공격 능력치를 1랭크 상승시킨다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		attacker.TakeEffect(attacker, defender, skill);
	}
}
