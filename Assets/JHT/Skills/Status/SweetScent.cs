using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SweetScent : SkillS
{
    public SweetScent() : base(
		"달콤한향기",
		"향기로 상대의 회피율을 크게 떨어뜨린다. 풀밭 등에서 쓰면 포켓몬이 다가온다.",
		0,
		SkillType.Status,
		false,
		PokeType.Normal,
		20,
		100
		) { }

	// 상대 포켓몬의 회피율을 1랭크 내린다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
