using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Psybeam : SkillS
{
    public Psybeam() : base(
		"환상빔",
		"이상한 광선을 상대에게 발사하여 공격한다. 상대를 혼란 시킬 때가 있다.",
		64,
		SkillType.Special,
		false,
		PokeType.Psychic,
		20,
		100
		) { }

	// 10%의 확률로 상대를 혼란 상태로 만든다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
			float effectRan = Random.Range(0f, 1f);
			if (effectRan < 0.1f)
			{
				defender.condition = StatusCondition.Confusion;
			}
		}
	}
}
