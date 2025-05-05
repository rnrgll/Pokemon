using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BodySlam : SkillS
{
	public BodySlam() : base(
		"누르기",
		"몸 전체로 상대를 덮쳐눌러 공격한다. 마비 상태로 만들 때가 있다.", 
		85,
		SkillType.Physical,
		false,
		PokeType.Normal,
		15,
		100
		) { }

	// 상대를 30% 확률로 마비 상태로 만든다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		// 명중 체크를 포켓몬 클래스에서 검사
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
			float effectRan = Random.Range(0f, 1f);
			if (effectRan < 0.3f)
			{
				defender.TakeEffect(attacker, defender, skill);
			}
		}
	}
}
