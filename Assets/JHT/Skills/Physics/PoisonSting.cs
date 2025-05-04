using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PoisonSting : SkillS
{

	public PoisonSting() : base(
		"독침",
		"유독한 침으로 찌른다. 때때로 상대를 중독시킨다",
		15,
		SkillType.Physical,
		false,
		PokeType.Poison,
		35,
		100
		) { }

	// 30%의 확률로 상대방을 독 상태로 만든다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
			float effectRan = Random.Range(0f, 1f);
			if (effectRan < 0.3f)
			{
				defender.TakeEffect(attacker, defender, skill);
			}
		}
	}
}
