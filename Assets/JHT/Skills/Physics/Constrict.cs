using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Constrict : SkillS
{
	public Constrict() : base(
		"휘감기",
		"넝쿨이나 촉수 등으로 휘감아 미미한 피해를 준다. 때때로 상대의 스피드를 떨어트린다",
		10,
		SkillType.Physical,
		false,
		PokeType.Normal,
		35,
		100
		){ }

	// 10%의 확률로 상대의 스피드를 1랭크 떨어뜨린다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
			float effectRan = Random.Range(0f, 1f);
			if (effectRan < 0.1f)
			{
				defender.TakeEffect(attacker, defender, skill);
			}
		}
	}
}
