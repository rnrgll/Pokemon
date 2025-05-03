using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Hypnosis : SkillS
{
    public Hypnosis() :base(
		"최면술",
		"졸음을 유도하는 암시를 걸어서 상대를 잠듦 상태로 만든다.",
		0,
		SkillType.Status,
		false,
		PokeType.Psychic,
		20,
		55
		) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeEffect(attacker, defender, skill);
		}
	}
}
