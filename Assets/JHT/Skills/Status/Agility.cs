using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Agility : SkillS
{
    public Agility() : base(
		"고속이동",
		"힘을 빼고 몸을 가볍게 해서 고속으로 움직인다. 자신의 스피드를 크게 올린다.",
		0,
		SkillType.Status,
		true,
		PokeType.Psychic,
		30,
		100
		) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		attacker.TakeEffect(attacker, defender, skill);
	}
}
