using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class QuickAttack: SkillS
{
    public QuickAttack() : base(
		"전광석화",
		"눈에 보이지 않는 굉장한 속도로 상대에게 돌진한다. 반드시 선제 공격할 수 있다.",
		40,
		SkillType.Physical,
		false,
		PokeType.Normal,
		30,
		100
		) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
		}
	}
}
