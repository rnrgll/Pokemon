using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Grudge : SkillS
{
    public Grudge() : base(
		"원한",
		"상대의 마지막 기술에 앙심을 품어 기술을 선보일 기회를 줄여버린다",
		0,
		SkillType.Status,
		false,
		PokeType.Ghost,
		10,
		100
		) { }

	// 상대가 마지막으로 사용한 기술에 원한을 품어 그 기술의 PP를 4만큼 줄인다.
	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
