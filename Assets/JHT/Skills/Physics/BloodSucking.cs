using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BloodSucking : SkillS
{
    public BloodSucking() : base(
		"흡혈",
		"이빨로 깨물고 피를 빨아 생명력을 빼앗는다",
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
			Debug.Log($"{attacker.pokeName} 의 체력 {healAmount} 회복");
		}
	}
}
