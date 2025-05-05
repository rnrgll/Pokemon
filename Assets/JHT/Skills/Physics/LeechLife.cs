using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class LeechLife : SkillS
{
    public LeechLife() : base(
		"흡혈",
		"피를 빨아서 공격한다. 준 데미지의 절반을 HP로 회복한다.",
		20,
		SkillType.Physical,
		false,
		PokeType.Bug,
		15,
		100) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			int healAmount = attacker.Heal(attacker.GetTotalDamage(attacker, defender, skill));
			defender.TakeDamage(attacker, defender, skill);
			Debug.Log($"배틀로그 : {attacker.pokeName} 의 체력 {healAmount} 회복");
		}
	}
}
