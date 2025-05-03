using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Swift : SkillS
{
	public Swift() : base(
		"스피드스타",
		"별 모양의 빛을 발사해서 상대를 공격한다. 공격은 반드시 명중한다.",
		60,
		SkillType.Special,
		false,
		PokeType.Normal,
		20,
		100	// 항상 적중
		) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		// 항상 적중
		defender.TakeDamage(attacker, defender, skill);
	}
}
