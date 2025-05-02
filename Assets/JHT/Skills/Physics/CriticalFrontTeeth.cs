using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CriticalFrontTeeth : SkillPhysic
{
    public CriticalFrontTeeth() : base("필살앞니", "날카로운 앞니로 콱 물어 본때를 보여 떄때로 상대를 풀이 죽게 한다",
		80, false, SkillType.Physical,PokeType.Normal,15,89.45f) { }

	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			defender.TakeDamage(attacker, defender, skill); //skill.damage* attacker.pokemonStat.attack
			if(rand <= 10)
			{
				defender.pokemonStat.attack -= 1;
			}
			skill.curPP--;
			//if(전투 초기화시)
			//{
			//	초기화
			//}
		}
		else
		{
			skill.curPP--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
