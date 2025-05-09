using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Explosion : SkillS
{
    public Explosion() : base(
		"대폭발",
		"커다란 폭발로 주위에 있는 모든 것을 공격한다. 사용한 다음 기절한다.",
		250,
		SkillType.Physical,
		false,
		PokeType.Normal,
		5,
		100
		){ }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
			attacker.hp = 0;
			attacker.isDead = true;
		}
	}
}
