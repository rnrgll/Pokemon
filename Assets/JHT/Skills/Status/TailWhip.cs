using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TailWhip : SkillS
{
    public TailWhip() : base(
		"꼬리흔들기",
		"꼬리를 좌우로 귀엽게 흔들어 방심을 유도한다. 상대의 방어를 떨어뜨린다.",
		0,
		SkillType.Status,
		false,
		PokeType.Normal,
		30,
		100
		) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
