using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Slam : SkillS
{
    public Slam() : base(
		"힘껏치기",
		"긴 꼬리나 덩굴 등을 사용해 상대를 힘껏 쳐서 공격한다.",
		80,
		SkillType.Physical,
		false,
		PokeType.Normal,
		20,
		75
		) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
		}
	}
}
