using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Scratch : SkillS
{
	public Scratch() : base(
		"할퀴기",
		"단단하고 뾰족한 날카로운 손톱으로 상대를 할퀴어서 공격한다.", 
		40,
		SkillType.Physical,
		false,
		PokeType.Normal,
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
