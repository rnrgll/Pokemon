using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Anger : SkillPhysic
{
	private int count = 0;
	public Anger() : base("분노", "공격 받을수록 점점 더 격한 분노를 표출한다",
		20, false, SkillType.Physical,PokeType.Normal,20,100) { }

	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);
		
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			defender.TakeDamage(attacker,defender, skill);
			skill.curPP--;
			if (count >= 1)
			{
				skill.damage += 1;
			}
			count++;

			//다른스킬을 사용할경우 스킬 데미지 초기화
			//if
			//{
			//	skill.damage -= count;
			//}
		}
		else
		{
			skill.curPP--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
