using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class DefenseCurl : SkillS
{
    public DefenseCurl() : base(
		"웅크리기",
		"몸을 둥글게 웅크려서 자신의 방어를 올린다.", 
		0,
		SkillType.Status,
		true,
		PokeType.Normal,
		40,
		100
		) { }

	// 자신의 방어력을 1랭크 올린다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		attacker.TakeEffect(attacker, defender, skill);
	}
}
