using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class DestinyBond : SkillS
{
    public DestinyBond() : base(
		"길동무",
		"자신을 쓰러트린 적 포켓몬을 길동무 삼아 같이 기절한다",
		0,
		SkillType.Status,
		false,
		PokeType.Ghost,
		5,
		100
		) { }

	// 자신을 길동무 상태로 만든다. 길동무 상태인 포켓몬을 기절시킨 포켓몬은 함께 기절하게 된다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		attacker.TakeEffect(attacker, defender, skill);
	}
}
