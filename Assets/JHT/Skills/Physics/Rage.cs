using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Rage : SkillS
{

	public Rage() : base(
			"분노",
			"기술을 썼을 때 공격을 받으면 분노의 힘으로 주는 대미지가 올라간다.",
			20,
			SkillType.Physical,
			false,
			PokeType.Normal,
			20,
			100
			){ }

	/*
	 데미지를 받을 때마다 데미지가 2배, 3배, 4배...로 점점 증가한다. 다른 기술을 사용하면 원래대로 돌아온다.
	*/

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		// 명중 체크를 포켓몬 클래스에서 검사
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
		}
	}
}
