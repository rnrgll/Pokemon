using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillS : MonoBehaviour
{
	public Sprite icon;
	public int damage;
	public string name;
	public string description;
	public PokeType pokeType;
	public GameObject particle;

	public SkillS(Sprite _icon, int _damage, string _name, string _description, PokeType _pokeType, Animator anim)
	{
		this.icon = _icon;
		this.name = _name;
		this.description = _description;
		this.pokeType = _pokeType;
		this.damage = _damage;
	}


	public void Attack(PokemonS attacker, PokemonS defender, Skill skill)
	{
		int rand = Random.Range(0,10);
		attacker.animator.SetTrigger(name); // 상의해보고 결정

		if (rand > 2)
		{
			defender.TakeDamage(attacker, skill.Power * attacker.pokemonStat.attack);
			Instantiate(particle, defender.transform.position, Quaternion.identity);

		}
		else
		{
			Debug.Log("공격을 회피하였습니다");
		}
	}
}
