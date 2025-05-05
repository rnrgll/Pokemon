using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Harden : SkillS
{
    public Harden() : base(
		"단단해지기",
		"전신에 힘을 담아 몸을 단단하게 해서 자신의 방어를 올린다.", 
		0,
		SkillType.Status,
		true,
		PokeType.Normal,
		30,
		100
		) { }

	// 방어를 1랭크 올린다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		attacker.TakeEffect(attacker, defender, skill);
	}
}
