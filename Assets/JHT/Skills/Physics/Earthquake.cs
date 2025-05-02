using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Earthquake : SkillPhysic
{
    public Earthquake() : base("지진", "강한 지진을 일으켜 주변 땅에 있는 것들에 피해를 준다",
		100, false, SkillType.Physical,PokeType.Ground,10,100) { }

	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			if (defender.pokeType2 == PokeType.Flying) return;
			defender.TakeDamage(attacker, defender, skill); //skill.damage* attacker.pokemonStat.attack
			skill.pp--;
		}
		else
		{
			skill.pp--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
