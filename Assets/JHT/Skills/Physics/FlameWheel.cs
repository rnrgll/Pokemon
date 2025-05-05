using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FlameWheel : SkillS
{
	public FlameWheel() : base(
		"화염자동차",
		"불꽃을 둘러 상대에게 돌진 공격한다. 화상 상태로 만들 때가 있다.",
		60,
		SkillType.Physical,
		false,
		PokeType.Fire,
		25,
		100
		)
	{ }

	/*
		10%의 확률로 화상 상태로 만든다.
		상대가 얼음 상태라면, 녹일 수 있다.
	 */

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
			// 얼음 해제
			defender.RestoreStatus(StatusCondition.Freeze);

			// 10% 화상
			float effectRan = Random.Range(0f, 1f);
			if (effectRan < 0.1f)
			{
				defender.TakeEffect(attacker, defender, skill);
			}
		}
	}
}
