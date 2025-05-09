using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SandAttack : SkillS
{
	public SandAttack() : base(
		"모래뿌리기",
		"상대의 얼굴에 모래를 뿌려서 명중률을 떨어뜨린다.",
		0,
		SkillType.Status,
		false,
		PokeType.Ground,
		15,
		100
		)
	{ }

	// 상대의 명중률이 1랭크 내려간다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
