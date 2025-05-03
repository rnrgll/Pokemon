using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Foresight : SkillS
{
    public Foresight() : base(
		"꿰뚫어보기",
		"고스트타입에 효과가 없는 기술이나 회피율이 높은 상대라 할지라도 공격이 맞게 된다.", 
		0,
		SkillType.Status,
		false,
		PokeType.Normal,
		40,
		100
		) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		defender.TakeEffect(attacker, defender, skill);
	}
}
