using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Leer : SkillS
{
	public Leer() : base(
		"째려보기",
		"날카로운 눈초리로 겁을 주어 상대의 방어를 떨어뜨린다.",
		0,
		SkillType.Status,
		false,
		PokeType.Normal,
		30,
		100
		)
	{ }

	// 상대의 방어력을 1랭크 떨어뜨린다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
