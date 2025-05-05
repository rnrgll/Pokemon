using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class RazorLeaf : SkillS
{
    public RazorLeaf() : base(
		"잎날가르기",
		"잎사귀를 날려 상대를 베어 공격한다. 급소에 맞을 확률이 높다.", 
		55,
		SkillType.Physical,
		false,
		PokeType.Grass,
		25,
		95
		) { }

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
