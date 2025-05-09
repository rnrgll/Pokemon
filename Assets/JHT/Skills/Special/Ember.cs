using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Ember : SkillS
{
    public Ember() : base(
		"불꽃세례",
		"작은 불꽃을 상대에게 발사하여 공격한다. 화상 상태로 만들 때가 있다.",
		40,
		SkillType.Special,
		false,
		PokeType.Fire,
		25,
		100
		) { }

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
