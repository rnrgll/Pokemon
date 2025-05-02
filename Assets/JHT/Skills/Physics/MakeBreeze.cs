using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MakeBreeze : SkillS
{
    public MakeBreeze() : base(
		"바람일으키기",
		"세찬 바람을 일으켜 적을 타격한다",
		40,
		SkillType.Physical,
		false,
		PokeType.Flying,
		35,
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
