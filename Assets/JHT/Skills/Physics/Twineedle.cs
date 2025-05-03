using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using static Define;

public class Twineedle : SkillS
{
    public Twineedle() : base(
		"더블니들",
		"2개의 침을 상대에게 꿰찔러 공격한다. 2회 연속으로 공격한다.",
		25,
		SkillType.Physical,
		false,
		PokeType.Bug,
		20,
		100
		) { }

	// 20%의 확률로 상대를 독 상태에 빠트린다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			// 어떤식으로 2번 때리지 2번호출하면되나
			defender.TakeDamage(attacker, defender, skill);

			defender.TakeDamage(attacker, defender, skill);

			float effectRan = Random.Range(0f, 1f);
			if (effectRan > 0.2f)
			{
				defender.condition = StatusCondition.Poison;
			}
		}
	}
}
