using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Dash : SkillS
{
    public Dash() : base(
		"돌진",
		"자신도 다칠 수 있지만 앞뒤를 가리지 않고 돌진해 들이 받는다",
		90,
		SkillType.Physical,
		false,
		PokeType.Normal,
		20,
		85
		) { }

	// 상대방에게 준 데미지의 1/4만큼 자신도 대미지를 입는다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			int totalDamage = defender.GetTotalDamage(attacker, defender, skill);
			defender.TakeDamage(attacker, defender, skill);
			attacker.hp -= totalDamage / 4;
		}
	}
}
