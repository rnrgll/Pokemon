using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Psychic : SkillS
{
    public Psychic() : base(
		"사이코키네시스",
		"강한 염동력을 상대에게 보내어 공격한다. 상대의 특수방어를 떨어뜨릴 때가 있다.",
		90,
		SkillType.Special,
		false,
		PokeType.Psychic,
		10,
		90
		) { }

	// 10%의 확률로 상대방의 특수방어를 1랭크 하락시킨다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
			float effectRan = Random.Range(0f, 1f);
			if (effectRan < 0.1f)
			{
				defender.TakeEffect(attacker, defender, skill);
			}
		}
	}
}
