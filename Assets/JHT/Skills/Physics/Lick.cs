using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Lick : SkillPhysic
{
	public Lick() : base("핥기", "혀로 소름 끼치게 핥는다. 때때로 상대를 마비시킨다", 20, false, SkillType.Physical) { }

	//public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	//{
	//	int rand = Random.Range(0, 10);
	//	defender.animator.SetTrigger(name);
	//
	//	//20 확률로 피함
	//	if (rand > 2)
	//	{
	//		defender.TakeDamage(attacker, defender, skill); //skill.damage* attacker.pokemonStat.attack
	//
	//	}
	//	else
	//	{
	//		Debug.Log("공격을 회피하였습니다");
	//	}
	//}
}
