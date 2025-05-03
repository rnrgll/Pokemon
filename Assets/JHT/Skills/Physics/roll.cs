using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Roll : SkillS
{
	int count =0;
	int curpp = 0;
    public Roll() : base(
		"구르기",
		"스스로 걷잡을 수 없이 계속 구른다. 미리 웅크렸다면 더욱 거세진다",
		30,
		SkillType.Physical,
		false,
		PokeType.Rock,
		20,
		90
		) { }

	/*
	매 턴마다 2배씩 위력이 증가한다.

	(30 > 60 > 120 > 240 > 480)
	*/

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
		}	
	}
}
