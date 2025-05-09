using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class StringShot : SkillS
{
    public StringShot() : base(
		"실뿜기",
		"입에서 뿜어낸 실을 휘감아서 상대의 스피드를 떨어뜨린다.",
		0,
		SkillType.Status,
		false,
		PokeType.Bug,
		40,
		95
		) { }

	// 상대의 스피드를 1랭크 떨어뜨린다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
