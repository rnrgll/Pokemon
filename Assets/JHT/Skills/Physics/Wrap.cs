using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Wrap : SkillS
{
	
    public Wrap() : base(
		"김밥말이",
		"긴 몸이나 덩굴을 사용해 2-5턴 동안 상대를 휘감아 공격한다.",
		15,
		SkillType.Physical,
		false,
		PokeType.Normal,
		20,
		85
		) { }

	// 이 기술에 걸린 포켓몬은 2 ~ 5턴동안 체력이 매턴 최대 체력의 1/16만큼 감소한다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
			defender.TakeEffect(attacker, defender, skill);
		}
	}

}
