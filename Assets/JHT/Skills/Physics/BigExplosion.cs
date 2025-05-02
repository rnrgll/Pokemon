using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BigExplosion : SkillPhysic
{
    public BigExplosion() : base("대폭발", "큰 폭발을 일으켜 주변에 피해를 주고 자신은 전투불능이 된다",
		250, false, SkillType.Physical,PokeType.Normal,5,100)
	{ }

	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			defender.TakeDamage(attacker, defender, skill);
			attacker.pokemonStat.hp = Mathf.Max(0, 0);
		}
		else
		{
			skill.curPP--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
