using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillPhysic : SkillS
{
	public SkillPhysic(string name, string description, float damage, bool isMyStat, SkillType skillType, PokeType type, int pp, float accuracy, bool isHm = false) :
		base(name, description, damage, skillType, isMyStat, type, pp, accuracy, isHm)
	{ }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		
		int rand = Random.Range(0, 100);
		//defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			//defender.TakeDamage(attacker, defender, skill); //skill.damage* attacker.pokemonStat.attack
			skill.curPP--;
		}
		else
		{
			skill.curPP--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
