using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NeedleMissile : SkillPhysic
{
    public NeedleMissile() : base("바늘미사일", "뾰족한 침을 마구 발사하여 2~5회 연속으로 상대를 찌른다",
		14, false, SkillType.Physical,PokeType.Bug,20,84.38f) { }

	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		int attackRand = Random.Range(2, 6);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			for (int i = 0; i <= attackRand; i++)
			{
				defender.TakeDamage(attacker, defender, skill);
			}
			skill.curPP--;
		}
		else
		{
			skill.curPP--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
