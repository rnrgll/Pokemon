using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Scratch : SkillPhysic
{
	public Scratch() : base("할퀴기", "단단하고, 뾰족하면서 날카로운 손톱이나 발톱으로 상대를 할퀸다.", 
		40, false, SkillType.Physical) { }
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
