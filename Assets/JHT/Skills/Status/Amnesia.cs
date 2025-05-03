using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Amnesia : SkillS
{
    public Amnesia() : base(
		"망각술",
		"머리를 비워서 순간적으로 무언가를 잊어버림으로써 자신의 특수방어를 크게 올린다.",
		0,
		SkillType.Status,
		true,
		PokeType.Psychic,
		20,
		100
		) { }

	// 자신의 특수방어력이 2랭크 상승한다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		attacker.TakeEffect(attacker, defender, skill);
	}
}
