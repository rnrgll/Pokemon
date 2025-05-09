using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Rollout : SkillS
{
    public Rollout() : base(
		"구르기",
		"스스로 걷잡을 수 없이 계속 구른다. 미리 웅크렸다면 더욱 거세진다", // 5턴 동안 구르기를 반복하여 공격한다. 기술이 맞을 때마다 위력이 올라간다.
		30,
		SkillType.Physical,
		false,
		PokeType.Rock,
		20,
		90
		) { }

	/*
	매 턴마다 2배씩 위력이 증가한다.

	(30 > 60 > 120 > 240 > 480)
	*/

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		int ran = Random.Range(0, 100);
		bool isHit = defender.TryHit(attacker, defender, skill);
		if (isHit)
		{
			defender.TakeDamage(attacker, defender, skill);
		}
		else
		{
			attacker.isRollout = false;
			attacker.rolloutStack = 1;
			Debug.Log($"배틀로그 : {attacker.pokeName} 의 {skill.name} 는 빗나감! 대미지 초기화 {ran}");
		}
	}
}
