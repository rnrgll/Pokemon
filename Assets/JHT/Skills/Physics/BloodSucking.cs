using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BloodSucking : SkillPhysic
{
    public BloodSucking() : base("흡혈", "이빨로 깨물고 피를 빨아 생명력을 빼앗는다",
		20, false, SkillType.Physical,PokeType.Bug,15,100) { }

	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			defender.TakeDamage(attacker, defender, skill);
			skill.curPP--;
			if (skill.damage > 2)
			{
				attacker.pokemonStat.hp = Mathf.Min(attacker.maxHp, (int)(attacker.pokemonStat.hp + skill.damage / 2));
			}
			else
			{
				attacker.pokemonStat.hp += 1;
			}
		}
		else
		{
			skill.curPP--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
