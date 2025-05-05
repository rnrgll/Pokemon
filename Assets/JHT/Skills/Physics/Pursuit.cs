using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Pursuit : SkillS
{
    public Pursuit() : base(
		"따라가때리기",
		"상대 포켓몬이 교체될 때 기술을 쓰면 2배의 위력으로 공격할 수 있다.",
		40,
		SkillType.Physical,
		false,
		PokeType.Dark,
		20,
		100
		) { }


	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		// 상대 포켓몬이 교체인걸 알아야함 > 상대는 교체를 안하게 설계
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
		}
	}
}
