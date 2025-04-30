using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PokemonStatS
{
	public int hp;
	public int attack;
	public int defense;
	public int speAttack;
	public int speDefense;
	public int speed;

	public PokemonStatS(int hp, int attack, int defense, int speAttack, int speDefense, int speed)
	{
		this.hp = hp;
		this.attack = attack;
		this.defense = defense;
		this.speAttack = speAttack;
		this.speDefense = speDefense;
		this.speed = speed;
	}
}
