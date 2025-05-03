using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Screech : SkillS
{
    public Screech() : base(
		"싫은소리",
		"그만 귀를 막아버리고 싶은 싫은 소리를 내어 상대의 방어를 크게 떨어뜨린다.",
		0,
		SkillType.Status,
		false,
		PokeType.Normal,
		40,
		85
		) { }

	// 상대의 방어력을 2랭크 떨어뜨린다.
	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
