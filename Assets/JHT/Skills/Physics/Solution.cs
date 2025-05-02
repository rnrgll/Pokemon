using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution : SkillPhysic
{
	public Solution() : base("용해액", "부식성의 강한 산성액을 뿌린다. 때때로 상대의 방어를 떨어트린다",
		40, false, Define.SkillType.Physical,Define.PokeType.Poison,30,100) { }
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
