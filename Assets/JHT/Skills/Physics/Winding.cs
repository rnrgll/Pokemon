using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Winding : SkillPhysic
{
	public Winding() : base("휘감기", "넝쿨이나 촉수 등으로 휘감아 미미한 피해를 준다. 때때로 상대의 스피드를 떨어트린다",
		10, false, SkillType.Physical, PokeType.Normal, 25, 100)
	{ }

	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			defender.TakeDamage(attacker, defender, skill); //skill.damage* attacker.pokemonStat.attack
			skill.pp--;
			if (rand < 10)
			{
				defender.pokemonStat.speed -= 1;
			}
		}
		else
		{
			skill.pp--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
