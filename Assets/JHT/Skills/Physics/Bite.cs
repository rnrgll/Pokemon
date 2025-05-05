using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Bite : SkillS
{
    public Bite() : base(
		"물기",
		"날카롭고 뾰족한 이빨로 물어서 공격한다. 상대를 풀이 죽게 할 때가 있다.",
		60,
		SkillType.Physical,
		false,
		PokeType.Dark,
		25,
		100
		) { }

	// 30%의 확률로 적을 풀죽게 한다.

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
