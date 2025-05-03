using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class WaterGun : SkillS
{
    public WaterGun() : base(
		"물대포",
		"물을 기세 좋게 상대에게 발사하여 공격한다.",
		40,
		SkillType.Special,
		false,
		PokeType.Water,
		25,
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
