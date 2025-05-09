using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Selfdestruct : SkillS
{
    public Selfdestruct() : base(
		"자폭",
		"스스로 폭발하여 주변에 피해를 주고 자신은 전투불능이 된다",
		200,
		SkillType.Physical,
		false,
		PokeType.Normal,
		5,
		100
		) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
			attacker.hp = 0;
			attacker.isDead = true;
		}
	}
}
