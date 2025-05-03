using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Supersonic : SkillS
{
    public Supersonic() : base(
		"초음파",
		"특수한 음파를 몸에서 발산하여 상대를 혼란 상태로 만든다.",
		0,
		SkillType.Status,
		false,
		PokeType.Normal,
		20,
		55
		) { }

	// 상대를 혼란 상태로 만든다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
