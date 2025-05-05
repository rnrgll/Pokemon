using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TakeDown: SkillS
{
    public TakeDown() : base(
		"돌진",
		"굉장한 기세로 상대에게 부딪쳐 공격한다. 자신도 조금 데미지를 입는다.",
		90,
		SkillType.Physical,
		false,
		PokeType.Normal,
		20,
		85
		) { }

	// 상대방에게 준 데미지의 1/4만큼 자신도 대미지를 입는다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			int totalDamage = defender.GetTotalDamage(attacker, defender, skill);
			defender.TakeDamage(attacker, defender, skill);
			int reboundDamage = totalDamage / 4;
			attacker.hp -= reboundDamage;
			Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 반동으로 {reboundDamage} 대미지를 입었다!");

			// 안에서 체크하니 그냥 실행
			attacker.GetPokemonDeadCheck();
		}
	}
}
