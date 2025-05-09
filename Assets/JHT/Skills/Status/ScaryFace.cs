using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ScaryFace : SkillS
{
    public ScaryFace() : base(
		"겁나는얼굴",
		"무서운 얼굴로 노려보고 겁주어 상대의 스피드를 크게 떨어뜨린다.",
		0,
		SkillType.Status,
		false,
		PokeType.Normal,
		10,
		90
		) { }

	// 상대의 스피드를 2랭크 떨어뜨린다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
