using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Press : SkillPhysic
{
	public Press(): base("누르기", "몸 전체의 무게로 짓누른다. 때때로 혈액순환을 방해해 마비시킨다", 
		85,false, SkillType.Physical,PokeType.Normal,15,100) { }
	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			defender.TakeDamage(attacker, defender, skill); //skill.damage* attacker.pokemonStat.attack
			skill.curPP--;
			if (rand <= 30)
			{
				//마비
			}
		}
		else
		{
			skill.curPP--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
