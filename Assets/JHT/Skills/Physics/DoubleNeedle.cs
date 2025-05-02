using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class DoubleNeedle : SkillPhysic
{
	int count = 0;
    public DoubleNeedle() :base("더블니들", "2개의 뾰족한 바늘로 연달아 찌른다. 때때로 상대를 중독시킨다",
		25, false, SkillType.Physical,PokeType.Bug,20,100) { }

	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			defender.TakeDamage(attacker, defender, skill); //skill.damage* attacker.pokemonStat.attack
			count++;
			defender.TakeDamage(attacker, defender, skill);
			skill.curPP--;
			if (count > 1 && rand < 20)
			{
				//독상태에 빠짐
			}

		}
		else
		{
			skill.curPP--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
