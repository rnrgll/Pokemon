using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Growl : SkillS
{
    public Growl() : base(
		"울음소리",
		"귀여운 울음소리를 들려주고 관심을 끌어 방심한 사이에 상대의 공격을 떨어뜨린다.",
		0,
		SkillType.Status,
		false,
		PokeType.Normal,
		40,
		100
		) { }

	// 상대의 공격력을 1랭크 떨어 뜨린다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
