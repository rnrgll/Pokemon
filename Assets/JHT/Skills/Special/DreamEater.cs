using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class DreamEater : SkillS
{
    public DreamEater() : base(
		"꿈먹기",
		"잠자고 있는 상대의 꿈을 먹어 공격한다. 데미지의 절반을 HP로 회복한다.",
		100,
		SkillType.Special,
		false,
		PokeType.Psychic,
		15,
		100
		){ }

	// 상대가 잠듦상태가 아니면 무조건 실패한다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			if (defender.condition == StatusCondition.Sleep)
				defender.TakeDamage(attacker, defender, skill);
			else
				Debug.Log($"배틀로그 : {defender.pokeName} 은/는 수면 상태가 아니라 {skill.name} 기술 실패!");
		}
	}
}
