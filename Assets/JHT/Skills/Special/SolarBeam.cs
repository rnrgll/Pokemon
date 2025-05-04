using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SolarBeam : SkillS
{
    public SolarBeam() : base(
		"솔라빔",
		"1턴째에 빛을 가득 모아 2턴째에 빛의 다발을 발사하여 공격한다.",
		120,
		SkillType.Special,
		false,
		PokeType.Grass,
		10,
		100
		) { }


	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		if (defender.TryHit(attacker, defender, skill))
		{
			defender.TakeDamage(attacker, defender, skill);
		}
	}
}
