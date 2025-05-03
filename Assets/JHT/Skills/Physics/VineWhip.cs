using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class VineWhip : SkillS
{
	public VineWhip() : base(
		"덩굴채찍",
		"채찍처럼 휘어지는 가늘고 긴 덩굴로 상대를 힘껏 쳐서 공격한다.",
		35,
		SkillType.Physical,
		false,
		PokeType.Grass,
		10,
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
