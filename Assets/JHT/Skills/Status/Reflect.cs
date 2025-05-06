using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Reflect : SkillS
{
   public Reflect() : base(
	   "리플렉터",
	   "5턴 동안 이상한 장막을 쳐서 상대로부터 받는 물리공격의 데미지를 약하게 한다.",
	   0,
	   SkillType.Status,
	   true,
	   PokeType.Psychic,
	   20,
	   100
	   ) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		attacker.TakeEffect(attacker, defender, skill);
	}
}
