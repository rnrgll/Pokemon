using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class WingAttack : SkillS
{
    public WingAttack() : base(
		"날개치기",
		"크게 펼친 훌륭한 날개를 상대에게 부딪쳐서 공격한다.",
		60,
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
