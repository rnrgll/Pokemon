using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BodySlam : SkillPhysic
{
	public BodySlam() : base("몸통박치기", "몸전체를 이용해 들이받는다",
		35, false, SkillType.Physical,PokeType.Normal,35,94.53f) { }
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
