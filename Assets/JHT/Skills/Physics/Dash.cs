using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Dash : SkillPhysic
{
    public Dash() : base("돌진", "자신도 다칠 수 있지만 앞뒤를 가리지 않고 돌진해 들이 받는다",
		90, false, SkillType.Physical,PokeType.Normal,20,84.38f) { }

	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			defender.TakeDamage(attacker, defender, skill); //skill.damage* attacker.pokemonStat.attack
			attacker.pokemonStat.hp = Mathf.Max(0, (int)(attacker.pokemonStat.hp - skill.damage/4));
			skill.pp--;
		}
		else
		{
			skill.pp--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
