using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MudSlap : SkillS
{
	public MudSlap() : base(
		"진흙뿌리기",
		"상대의 얼굴 등에 진흙을 내던져서 공격한다. 명중률을 떨어뜨린다.",
		20,
		SkillType.Special,
		false,
		PokeType.Ground,
		10,
		100
		)
	{ }

	// 상대의 명중률이 1랭크 내려간다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
