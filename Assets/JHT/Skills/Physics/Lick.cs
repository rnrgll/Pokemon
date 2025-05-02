using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Lick : SkillS
{
	public Lick() : base(
		"핥기",
		"혀로 소름 끼치게 핥는다. 때때로 상대를 마비시킨다",
		20,
		SkillType.Physical,
		false,
		PokeType.Ghost,
		30,
		100) { }

	/*
	 * 30%의 확률로 상대를 마비 상태로 만든다.
	 */

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
			float effectRan = Random.Range(0f, 1f);
			if (effectRan < 0.1f)
			{
				defender.condition = StatusCondition.Paralysis;
			}
		}
	}
}
