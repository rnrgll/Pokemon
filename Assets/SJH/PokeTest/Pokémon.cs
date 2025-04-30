using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokémon : MonoBehaviour
{
	public int id;
	public string pokeName;
	public int level;
	public int hp;
	public int maxHp;
	public int curExp;
	public int nextExp;
	[SerializeField] public PokemonStat baseStat;   // 종족값
	[SerializeField] public PokemonIV iv;           // 개체값
													//public PokemonStat ev;			// 노력치
	[SerializeField] public PokemonStat pokemonStat;    // 현재수치
	public PokeType pokeType1;
	public PokeType pokeType2;

	public bool isDead;



	// 생성자는 사용할 수 없으니 Init함수를 실행해서 데이터 할당
	public void Init(int _id, int _level)
	{
		// 데이터 매니저에서 고정데이터 받아오기
		SJH_PokemonData data = Manager.Data.SJH_PokemonData.GetPokemonData(_id);

		// 고정데이터 Id, 이름, 종족값, 타입
		id = data.Id;
		pokeName = data.Name;
		baseStat = data.BaseStat;
		pokeType1 = data.PokeType1;
		pokeType2 = data.PokeType2;

		// 개별데이터 hp exp iv stat
		level = _level;
		isDead = false;
		iv = PokemonIV.GetRandomIV();
		pokemonStat = GetStat();
		hp = pokemonStat.hp;
		maxHp = hp;
	}
	public void Init(string _name, int _level)
	{
		SJH_PokemonData data = Manager.Data.SJH_PokemonData.GetPokemonData(_name);
		id = data.Id;
		pokeName = data.Name;
		baseStat = data.BaseStat;
		pokeType1 = data.PokeType1;
		pokeType2 = data.PokeType2;

		// hp exp iv stat
		level = _level;
		isDead = false;
		iv = PokemonIV.GetRandomIV();
		pokemonStat = GetStat();
		hp = pokemonStat.hp;
		maxHp = hp;
	}

	// 모든 값 개별적으로 입력
	public void Init(int _id, string _name, int _level, PokemonStat _baseStat, PokemonIV _iv, PokeType _pokeType1, PokeType _pokeType2)
	{
		id = _id;
		pokeName = _name;
		level = _level;
		baseStat = _baseStat;
		iv = _iv;
		pokeType1 = _pokeType1;
		pokeType2 = _pokeType2;

		pokemonStat = GetStat();
		curExp = 0;
		nextExp = 999;
		hp = pokemonStat.hp;
		maxHp = hp;

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
