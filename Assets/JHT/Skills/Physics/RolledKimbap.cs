using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class RolledKimbap : SkillS
{
	
    public RolledKimbap() : base(
		"김밥말이",
		"긴 몸이나 넝쿨 등으로 말아 묶어서 지속적인 피해를 준다",
		15,
		SkillType.Physical,
		false,
		PokeType.Normal,
		20,
		85
		) { }

	// 2~5턴동안 지속

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
			int effectRan = Random.Range(2, 6);
			// TODO : 포켓몬 김밥말이 상태 추가 필요 Bind
			// 김밥말이 상태일 때는 교체 및 도망가기 불가하게
		}
	}

}
