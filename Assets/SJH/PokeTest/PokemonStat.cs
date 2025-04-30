using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PokemonStat
{
	public int hp;
	public int attack;
	public int defense;
	public int speAttack;
	public int speDefense;
	public int speed;

	public PokemonStat(int hp, int attack, int defense, int speAttack, int speDefense, int speed)
	{
		this.hp = hp;
		this.attack = attack;
		this.defense = defense;
		this.speAttack = speAttack;
		this.speDefense = speDefense;
		this.speed = speed;
	}
}
