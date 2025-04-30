using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillS : MonoBehaviour
{
	//public Sprite icon;
	public int damage;
	public string name;
	public string description;
	public PokeType pokeType;
	public GameObject particle;

	public SkillS(int _damage, string _name, string _description, PokeType _pokeType)
	{
		this.name = _name;
		this.description = _description;
		this.pokeType = _pokeType;
		this.damage = _damage;
	}

	public void AttackStatus(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		attacker.TakeStatus(attacker, skill.damage);
	}

	public void Attack(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		int rand = Random.Range(0,10);
		attacker.animator.SetTrigger(name);

		
		if (rand > 2)
		{
			defender.TakeDamage(attacker, skill.damage * attacker.pokemonStat.attack);
			GameObject parti = Instantiate(particle, defender.transform.position, Quaternion.identity);
			Destroy(parti, 2f);

		}
		else
		{
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
