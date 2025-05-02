using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class stinger : SkillPhysic
{

	public stinger() : base("독침", "유독한 침으로 찌른다. 때때로 상대를 중독시킨다", 
		15, false, SkillType.Physical,PokeType.Poison,35,100) { }

	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			defender.TakeDamage(attacker, defender, skill); //skill.damage* attacker.pokemonStat.attack
			skill.curPP--;
			if (rand < 30)
			{
				//독
			}
		}
		else
		{
			skill.curPP--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
