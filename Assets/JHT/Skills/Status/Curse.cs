using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Curse : SkillS
{
	public Curse() : base(
		"저주",
		"기술을 쓰는 포켓몬이 고스트 타입일 때와 그 이외의 타입일 때 효과가 다르다.",
		0,
		SkillType.Status,
		false,
		PokeType.Ghost,
		10,
		100
		) { } //둘다 작용해야됨

	/*
		기술을 쓰는 포켓몬이 고스트 타입일 경우에는 자신의 HP 절반을 깎아 상대 포켓몬의 HP를 매턴 1/4씩 깎는다. 이 효과는 다른 포켓몬으로 교체하게 되면 사라진다.

		기술을 쓰는 포켓몬이 고스트 타입이 아닐 경우에는 자신의 스피드를 1랭크 하락시키고 공격/방어를 각각 1랭크씩 상승시킨다.
	 */

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		attacker.TakeEffect(attacker, defender, skill);
	}
}
