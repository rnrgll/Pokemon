using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NightShade : SkillS
{
	public NightShade() : base(
		"나이트헤드",
		"환상을 보게 해서 자신의 레벨과 똑같은 만큼의 데미지를 상대에게 준다.",
		0,
		SkillType.Special,
		false,
		PokeType.Ghost,
		15,
		100
		)
	{ }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
		}
	}
}
