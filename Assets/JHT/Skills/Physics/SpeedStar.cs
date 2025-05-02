using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SpeedStar : SkillPhysic
{
	public SpeedStar() : base("스피드스타", "빗나가지 않는 별 모양의 빛을 날린다",
		60, false, SkillType.Physical,PokeType.Normal,20,100) { }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (defender.pokeType2 == PokeType.Flying)
		{
			if(Mathf.RoundToInt(accuracy) >= rand)
			{
				defender.TakeDamage(attacker, defender, skill);
				skill.curPP--;
				return;
			}
			skill.curPP--;
			Debug.Log("공격을 회피하였습니다");
		}
		else if (defender.pokeType2 != PokeType.Flying)
		{
			defender.TakeDamage(attacker, defender, skill);
			skill.curPP--;
		}
		else
		{
			skill.curPP--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
