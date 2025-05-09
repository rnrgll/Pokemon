using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SmokeScreen : SkillS
{
    public SmokeScreen() : base(
		"연막",
		"연기나 먹물을 내뿜어 상대의 명중률을 떨어뜨린다.",
		0,
		SkillType.Status,
		false,
		PokeType.Normal,
		20,
		100
		) { }

	// 상대의 명중률을 1랭크 떨어뜨린다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
