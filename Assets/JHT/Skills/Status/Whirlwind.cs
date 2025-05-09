using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Whirlwind : SkillS
{
    public Whirlwind() : base(
		"날려버리기",
		"상대 포켓몬을 멀리 날려버린다",
		0,
		SkillType.Status,
		false,
		PokeType.Normal,
		20,
		100
		) { }

	// 상대방을 강제로 교체시킨다. 이때 교체되어 나오는 포켓몬은 무작위로 결정된다.

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		// TODO : 상대가 트레이너면 포켓몬 랜덤으로 강제 교체 > 상대가 트레이너고 1마리만 가지고 있으면 실패
		// 야생이면 배틀 종료
	}

}
