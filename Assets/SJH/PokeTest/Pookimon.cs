using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using static Define;

public class Pookimon
{
	public int id;
	public string name;
	public int level;
	public int hp;
	public int maxHp;
	public int curExp;
	public int nextExp;
	public PokemonStat baseStat;	// 종족값
	public PokemonIV iv;			// 개체값
	//public PokemonStat ev;			// 노력치
	public PokemonStat pokemonStat;	// 현재수치
	public PokeType pokeType1;
	public PokeType pokeType2;

	public bool isDead;

	public Pookimon(int _id, string _name, int _level, PokemonStat _baseStat, PokemonIV _iv, PokeType _pokeType1, PokeType _pokeType2)
	{
		id = _id;
		name = _name;
		level = _level;
		baseStat = _baseStat;
		iv = _iv;
		pokeType1 = _pokeType1;
		pokeType2 = _pokeType2;

		pokemonStat = GetStat();
		curExp = 0;
		nextExp = 999;
		hp = maxHp;

		isDead = false;
	}
	// 개체값 종족값 레벨을 계산해서 기본 스탯 반환
	private PokemonStat GetStat()
	{
		return new PokemonStat(
			hp: ((baseStat.hp * 2 + iv.hp) * level) / 100 + level + 10,
			attack: ((baseStat.attack * 2 + iv.attack) * level) / 100 + 5,
			defense: ((baseStat.defense * 2 + iv.defense) * level) / 100 + 5,
			speAttack: ((baseStat.speAttack * 2 + iv.speAttack) * level) / 100 + 5,
			speDefense: ((baseStat.speDefense * 2 + iv.speDefense) * level) / 100 + 5,
			speed: ((baseStat.speed * 2 + iv.speed) * level) / 100 + 5
		);
	}
}
