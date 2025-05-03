using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MeanLook : SkillS
{
    public MeanLook() : base(
		"검은눈빛",
		"빨려 들어갈 것 같은 까만 눈빛으로 가만히 응시하여 상대를 배틀에서 도망갈 수 없게 한다.", 
		0,
		SkillType.Status,
		false,
		PokeType.Normal,
		5,
		100
		) { }

	// 이 기술에 걸린 포켓몬은 다른 포켓몬과 교체하거나 도망갈 수 없게 된다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		defender.TakeEffect(attacker, defender, skill);
	}
}
