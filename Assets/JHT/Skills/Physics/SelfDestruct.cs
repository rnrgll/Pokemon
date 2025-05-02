using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SelfDestruct : SkillPhysic
{
    public SelfDestruct() : base("자폭", "스스로 폭발하여 주변에 피해를 주고 자신은 전투불능이 된다",
		200, false, SkillType.Physical,PokeType.Normal,5,100) { }

	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			defender.TakeDamage(attacker, defender, skill); //skill.damage* attacker.pokemonStat.attack
			attacker.pokemonStat.hp = Mathf.Max(0, 0);
			skill.curPP--;
		}
		else
		{
			skill.curPP--;
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
