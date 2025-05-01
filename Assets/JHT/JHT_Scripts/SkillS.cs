using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillS : MonoBehaviour
{
	//public Sprite icon;
	public string name;
	public string description;
	public int damage;
	public SkillType skillType;
	public GameObject Physicsparticle;
	public GameObject specialParticle;

	public SkillS(string _name, string _description, int _damage, SkillType _skillType)
	{
		this.name = _name;
		this.description = _description;
		this.damage = _damage;
		this.skillType = _skillType;
	}

	//스킬 사용시 Attack호출하고 attacker,defender,skill 사용시 타입별로 Attack실행
	public void Attack(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		if (skill.skillType == SkillType.Physical)
		{
			AttackPhysic(attacker, defender, skill);
		}
		else if (skill.skillType == SkillType.Special)
		{
			AttackSpecial(attacker,defender, skill);
		}
		else
		{
			AttackStat(attacker, defender, skill);
		}
	}
	public void AttackSpecial(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		//무슨효과?
	}

	public void AttackStat(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		attacker.TakeStat(attacker,defender, skill.damage);

		//파티클은 어떻게 실행할건가
		GameObject parti = Instantiate(specialParticle, defender.transform.position, Quaternion.identity);
		Destroy(parti, 2f);
	}

	public void AttackPhysic(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0,10);
		attacker.animator.SetTrigger(name);

		//20 확률로 피함
		if (rand > 2)
		{
			defender.TakeDamage(attacker,defender, skill.damage * attacker.pokemonStat.attack);
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
