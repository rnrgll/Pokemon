using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PokemonBattleStat
{
	// 스택용 -6 ~ 6
	public int attack;
	public int defense;
	public int speAttack;
	public int speDefense;
	public int speed;
	public int accuracy;
	public int evasion;
	public int critical;

	public PokemonBattleStat(int value)
	{
		attack = value;
		defense = value;
		speAttack = value;
		speDefense = value;
		speed = value;
		accuracy = value;
		evasion = value;
		critical = value;
	}
}
