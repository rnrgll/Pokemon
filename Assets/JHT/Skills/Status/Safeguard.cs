using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Safeguard : SkillS
{
    public Safeguard() : base(
		"신비의부적",
		"5턴 동안 이상한 힘의 보호를 받아 상태 이상이 되지 않는다.",
		0,
		SkillType.Status,
		true,
		PokeType.Normal,
		25,
		100
		) { }

	// 5턴 동안 우리 쪽 진영이 신비의부적에 둘러싸여 상태이상과 혼란 상태가 되지 않는다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		attacker.TakeEffect(attacker, defender, skill);
	}
}
