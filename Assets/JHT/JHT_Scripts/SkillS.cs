using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillS : MonoBehaviour
{
	//public Sprite icon;
	public string name;
	public string description;
	public float damage;
	public SkillType skillType;
	public bool isMyStat;
	public GameObject Physicsparticle;
	public GameObject specialParticle;


	public SkillS(string _name, string _description, float _damage,bool _isMyStat, SkillType _skillType)
	{
		this.name = _name;
		this.description = _description;
		this.damage = _damage;
		this.skillType = _skillType;
	}

	//스킬 사용시 Attack호출하고 attacker,defender,skill 사용시 타입별로 Attack실행
	public void Attack(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		if (skill.skillType == SkillType.Status)
		{
			AttackStat(attacker, defender, skill);
		}
		else
		{
			AttackDamage(attacker, defender, skill);
		}
	}

	public virtual void AttackStat(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		defender.TakeStat(attacker,defender, skill);

		//파티클은 어떻게 실행할건가
		GameObject parti = Instantiate(specialParticle, defender.transform.position, Quaternion.identity);
		Destroy(parti, 2f);
	}

	public void AttackDamage(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0,10);
		defender.animator.SetTrigger(name);

		//20 확률로 피함
		if (rand > 2)
		{
			defender.TakeDamage(attacker,defender, skill); //skill.damage* attacker.pokemonStat.attack
			//파티클은 어떻게 실행할건가
			GameObject parti = Instantiate(Physicsparticle, defender.transform.position, Quaternion.identity);
			Destroy(parti, 2f);

		}
		else
		{
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
