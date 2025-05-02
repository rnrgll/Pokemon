using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Scratching : SkillPhysic
{
	int attackRand = Random.Range(2, 6);

	public Scratching() : base("마구할퀴기", "뾰족하면서 날카로운 손톱이나 발톱으로 2~5회 연속으로 난도질한다",
		18, false, SkillType.Physical,PokeType.Normal,15,79.69f) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			if (Mathf.RoundToInt(accuracy) >= rand)
			{
				for (int i = 0; i <= attackRand; i++)
				{
					defender.TakeDamage(attacker, defender, skill);
				}
				skill.curPP--;
			}
		}
		else
		{
			skill.curPP--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
