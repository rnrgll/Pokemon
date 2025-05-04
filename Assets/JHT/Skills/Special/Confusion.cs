using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Confusion : SkillS
{
    public Confusion() : base(
		"염동력",
		"약한 염동력을 상대에게 보내어 공격한다. 상대를 혼란 시킬 때가 있다.",
		50,
		SkillType.Special,
		false,
		PokeType.Psychic,
		25
		,100
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
				defender.TakeEffect(attacker, defender, skill);
			}
		}
	}
}
