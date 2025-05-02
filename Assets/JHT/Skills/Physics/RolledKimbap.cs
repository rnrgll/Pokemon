using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class RolledKimbap : SkillPhysic
{
	
	int listRand = Random.Range(1, 11);
    public RolledKimbap() : base("김밥말이", "긴 몸이나 넝쿨 등으로 말아 묶어서 지속적인 피해를 준다",
		15, false, SkillType.Physical,PokeType.Normal,20,84.38f) { }


	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			defender.TakeDamage(attacker, defender, skill); //skill.damage* attacker.pokemonStat.attack
			skill.curPP--;
			if (listRand <= 4)
			{
				//2턴동안 속박
			}
			else if (listRand >= 5 && listRand < 9)
			{
				//3턴동안 속박
			}
			else if (listRand == 8)
			{
				//4턴동안 속박
			}
			else
			{
				//5턴동안 속박
			}

		}
		else
		{
			skill.curPP--;
			Debug.Log("공격을 회피하였습니다");
		}
	}

}
