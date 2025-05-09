using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class StunSpore : SkillS
{
	public StunSpore() : base(
		"저리가루",
		"저리 가루를 많이 흩뿌려서 상대를 마비 상태로 만든다.",
		0,
		SkillType.Status,
		false,
		PokeType.Grass,
		30,
		75
		)
	{ }

	// 상대를 마비 상태로 만든다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
