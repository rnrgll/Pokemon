using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FuryAttack : SkillS
{
	public FuryAttack() : base(
		"마구찌르기",
		"뿔이나 부리로 상대를 찔러서 공격한다. 2-5회 동안 연속으로 쓴다.",
		15,
		SkillType.Physical,
		false,
		PokeType.Normal,
		20,
		85
		)
	{ }

	/*
		명중시켰을 경우 기본 2회 공격하며 최대 5회까지 공격한다.
		37.5%의 확률로 2회까지 공격
		37.5%의 확률로 3회까지 공격
		12.5%의 확률로 4회까지 공격
		12.5%의 확률로 5회까지 공격
	 */

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			float effectRan = Random.Range(0f, 1f);
			int attackCount = 0;
			if (effectRan < 0.375f)
				attackCount = 2;
			else if (effectRan < 0.75f)
				attackCount = 3;
			else if (effectRan < 0.875f)
				attackCount = 4;
			else
				attackCount = 5;

			Debug.Log($"배틀로그 : {attacker.pokeName} 의 {skill.name} {attackCount}회 사용!");
			for (int i = 1; i <= attackCount; i++)
			{
				defender.TakeDamage(attacker, defender, skill);
				if (defender.hp <= 0)
					break;
			}
		}
	}
}
