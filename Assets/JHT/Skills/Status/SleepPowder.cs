using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SleepPowder : SkillS
{
	public SleepPowder() : base(
		"수면가루",
		"잠이 오는 가루를 많이 흩뿌려서 상대를 잠듦 상태로 만든다.",
		0,
		SkillType.Status,
		false,
		PokeType.Grass,
		15,
		75
		){ }

	// 상대를 잠듦 상태로 만든다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
