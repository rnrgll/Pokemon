using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PoisonPowder : SkillS
{
	public PoisonPowder() : base(
		"독가루",
		"유독한 가루를 뿌려 중독시킨다",
		0,
		SkillType.Status,
		false,
		PokeType.Poison,
		35,
		75
		) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
