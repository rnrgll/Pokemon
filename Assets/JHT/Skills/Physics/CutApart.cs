using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CutApart : SkillS
{
    public CutApart() : base(
		"베어가르기",
		"발톱이나 낫 등으로 상대를 베어 갈라서 공격한다. 급소에 맞을 확률이 높다.",
		70,
		SkillType.Physical,
		false,
		PokeType.Normal,
		20,
		100
		) { }

	// 6.25% > 12.5% 급소율 상승

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			//defender.TakeDamage(attacker, defender, skill);
			float effectRan = Random.Range(0f, 1f);
			if (effectRan < 0.125f) //attacker.pokemonBattleStack.critical + 1)
			{
				// 급소
				int totalDamage = defender.GetTotalDamage(attacker, defender, skill);
				defender.TakeDamage(totalDamage * 2);
			}
			else
				defender.TakeDamage(attacker, defender, skill);
		}
	}
}
