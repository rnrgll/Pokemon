using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Lick : SkillPhysic
{
	public Lick() : base("핥기", "혀로 소름 끼치게 핥는다. 때때로 상대를 마비시킨다",
		20, false, SkillType.Physical,PokeType.Ghost,30,100) { }

	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			defender.TakeDamage(attacker, defender, skill); //skill.damage* attacker.pokemonStat.attack
			skill.pp--;
			//if (rand <= 30)
			//{
			//	//마비
			//}
		}
		else
		{
			skill.pp--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
