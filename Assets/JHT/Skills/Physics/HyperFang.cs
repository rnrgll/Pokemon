using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class HyperFang : SkillS
{
    public HyperFang() : base(
		"필살앞니",
		"날카로운 앞니로 강하게 물어서 공격한다. 상대를 풀죽게 만들 때가 있다.",
		80,
		SkillType.Physical,
		false,
		PokeType.Normal,
		15,
		90
		) { }


	/*
	 * 10%의 확률로 상대를 풀죽게 한다.
	 */

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
			float effectRan = Random.Range(0f, 1f);
			if (effectRan < 0.1f)
			{
				defender.TakeEffect(attacker, defender, skill);
			}
		}
	}
}
