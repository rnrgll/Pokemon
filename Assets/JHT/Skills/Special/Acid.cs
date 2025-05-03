using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Acid : SkillS
{
	public Acid() : base(
		"용해액",
		"부식성의 강한 산성액을 뿌린다. 때때로 상대의 방어를 떨어트린다",
		40,
		SkillType.Special,
		false,
		PokeType.Poison,
		30,
		100
		) { }

	// 10% 의 확률로 상대의 특수방어력 을 1랭크 떨어 뜨린다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
			float effectRan = Random.Range(0f, 1f);
			if (effectRan < 0.1f)
			{
				defender.pokemonBattleStack.speDefense--;
			}
		}
	}
}
