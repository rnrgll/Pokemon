using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Synthesis : SkillS
{
    public Synthesis() : base(
		"광합성",
		"자신의 HP를 회복한다. 날씨에 따라 회복량이 변한다.",
		0,
		SkillType.Status,
		true,
		PokeType.Grass,
		5,
		100
		) { }

	// 날씨기능 없으니 50%회복
	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		attacker.TakeEffect(attacker, defender, skill);
	}
}
