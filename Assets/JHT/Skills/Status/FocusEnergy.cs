using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FocusEnergy : SkillS
{
    public FocusEnergy() : base(
		"기충전",
		"깊게 숨을 들이마셔 기합을 넣는다. 자신의 공격이 급소에 맞을 확률을 올린다.",
		0,
		SkillType.Status,
		true,
		PokeType.Normal,
		30,
		100
		) { }

	// 급소율을 1랭크 상승시킨다.
	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		attacker.TakeEffect(attacker, defender, skill);
	}
}
