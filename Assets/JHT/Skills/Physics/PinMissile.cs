using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PinMissile : SkillS
{
	public PinMissile() : base(
		"바늘미사일",
		"날카로운 침을 상대에게 발사해서 공격한다. 2-5회 동안 연속으로 쓴다.",
		14,
		SkillType.Physical,
		false,
		PokeType.Bug,
		20,
		84.38f) { }

	/*
		37.5%의 확률로 2회까지 공격
		37.5%의 확률로 3회까지 공격
		12.5%의 확률로 4회까지 공격
		12.5%의 확률로 5회까지 공격

		급소 매공격마다
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
