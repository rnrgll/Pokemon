using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Rest : SkillS
{
	public Rest() : base(
		"잠자기",
		"2턴 동안 계속 잠잔다. HP와 몸의 이상을 모두 회복한다.",
		0,
		SkillType.Status,
		false,
		PokeType.Psychic,
		5,
		100
		)
	{ }

	/*
		자신의 HP와 상태이상을 모두 회복하지만, 2턴 동안 잠듦 상태가 된다.
		HP가 가득 차 있을 때 사용하면 실패한다.
	*/

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		attacker.TakeEffect(attacker, defender, skill);
	}
}
