using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class LightScreen : SkillS
{
    public LightScreen() : base(
		"빛의장막",
		"5턴 동안 이상한 장막으로 상대로부터 받는 특수공격의 데미지를 약하게 한다.",
		0,
		SkillType.Status,
		true,
		PokeType.Psychic,
		30,
		100
		) { }

	// 5턴동안 상대로부터 받는 특수공격의 데미지를 반감시킨다.
	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		attacker.TakeEffect(attacker, defender, skill);
	}
}
