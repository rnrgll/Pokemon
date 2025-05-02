using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class SkillS
{
	//public Sprite icon;
	public string name;
	public string description;
	public float damage;
	public SkillType skillType;
	public bool isMyStat;


	public SkillS(string _name, string _description, float _damage,bool _isMyStat, SkillType _skillType)
	{
		this.name = _name;
		this.description = _description;
		this.damage = _damage;
		this.skillType = _skillType;
	}

	public abstract void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill);

	//스킬 사용시 Attack호출하고 attacker,defender,skill 사용시 타입별로 Attack실행
	//public void Attack(PokemonS attacker, PokemonS defender, SkillS skill)
	//{
	//	if (skill.skillType == SkillType.Status)
	//	{
	//		AttackStat(attacker, defender, skill);
	//	}
	//	else
	//	{
	//		AttackDamage(attacker, defender, skill);
	//	}
	//}
	//
	//public virtual void AttackStat(PokemonS attacker, PokemonS defender, SkillS skill)
	//{
	//	if (skill.isMyStat)
	//	{
	//		attacker.TakeMyStat(attacker, skill);
	//	}
	//	else
	//	{
	//		defender.TakeStat(attacker, defender, skill);
	//	}
	//	
	//}
	//
	//public void AttackDamage(PokemonS attacker, PokemonS defender, SkillS skill)
	//{
	//	int rand = Random.Range(0,10);
	//	defender.animator.SetTrigger(name);
	//
	//	//20 확률로 피함
	//	if (rand > 2)
	//	{
	//		defender.TakeDamage(attacker,defender, skill); //skill.damage* attacker.pokemonStat.attack
	//
	//	}
	//	else
	//	{
	//		Debug.Log("공격을 회피하였습니다");
	//	}
	//}
}
