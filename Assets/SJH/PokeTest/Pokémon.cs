using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Pokémon : MonoBehaviour
{
	[Tooltip("도감 번호")]
	public int id;
	[Tooltip("포켓몬 이름")]
	public string pokeName;
	[Tooltip("포켓몬 레벨")]
	public int level;
	[Tooltip("포켓몬 현재 체력")]
	public int hp;
	[Tooltip("포켓몬 최대 체력")]
	public int maxHp;
	[Tooltip("포켓몬 누적 경험치")]
	public int curExp;
	[Tooltip("포켓몬 다음 경험치")]
	public int nextExp;
	[Tooltip("포켓몬 종족값")]
	[SerializeField] public PokemonStat baseStat;
	[Tooltip("포켓몬 개체값")]
	[SerializeField] public PokemonIV iv;
	//public PokemonStat ev;	// 노력치
	[Tooltip("포켓몬 총 스탯")]
	[SerializeField] public PokemonStat pokemonStat;
	[Tooltip("포켓몬 타입 1")]
	public Define.PokeType pokeType1;
	[Tooltip("포켓몬 타입 2")]
	public Define.PokeType pokeType2;
	[Tooltip("포켓몬 경험치 타입")]
	public Define.ExpType expType;
	[Tooltip("죽었으면 true / 살았으면 false")]
	public bool isDead;



	// 생성자는 사용할 수 없으니 Init함수를 실행해서 데이터 할당
	public void Init(int _id, int _level)
	{
		// 데이터 매니저에서 고정데이터 받아오기
		SJH_PokemonData data = Manager.Data.SJH_PokemonData.GetPokemonData(_id);

		SetData(data, _level);
	}
	public void Init(string _name, int _level)
	{
		SJH_PokemonData data = Manager.Data.SJH_PokemonData.GetPokemonData(_name);
		SetData(data, _level);
	}

	private void SetData(SJH_PokemonData data, int _level)
	{
		// 고정데이터 Id, 이름, 종족값, 타입
		id = data.Id;
		pokeName = data.Name;
		baseStat = data.BaseStat;
		pokeType1 = data.PokeType1;
		pokeType2 = data.PokeType2;
		expType = data.ExpType;

		// 개별데이터 hp exp iv stat
		level = _level;
		curExp = 0;
		nextExp = GetNextExp();
		isDead = false;
		iv = PokemonIV.GetRandomIV();
		pokemonStat = GetStat();
		hp = pokemonStat.hp;
		maxHp = hp;
	}

	// 모든 값 개별적으로 입력
	public void Init(int _id, string _name, int _level, PokemonStat _baseStat, PokemonIV _iv, Define.PokeType _pokeType1, Define.PokeType _pokeType2)
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

	// 경험치 타입에 따라 다음 경험치 반환
	public int GetNextExp()
	{
		int curTotal = GetTotalExp(level);
		int nextTotal = GetTotalExp(level + 1);
		return nextTotal - curTotal;
	}

	public int GetTotalExp(int level)
	{
		switch (expType)
		{
			// 빠른 800,000			EXP = 4 * Level³ / 5
			case Define.ExpType.Fast: return (int)(4f * Mathf.Pow(level, 3) / 5f);
			// 약간 빠름 1,000,000	EXP = Level³
			case Define.ExpType.MediumFast: return (int)(Mathf.Pow(level, 3));
			// 약간 느림 1,059,860	EXP = (6/5) * Level³ - 15 * Level² + 100 * Level - 140
			case Define.ExpType.MediumSlow: return (int)((6f / 5f) * Mathf.Pow(level, 3) - 15 * Mathf.Pow(level, 2) + 100 * level - 140);
			// 약간 빠름 1,000,000	EXP = Level³
			default: return (int)(Mathf.Pow(level, 3));
		}
	}


	public void AddExp(int value)
	{
		curExp += value;

		// 누적 경험치가 초과하면 레벨업 반복
		while (curExp >= nextExp)
		{
			curExp -= nextExp;
			level++;
			nextExp = GetNextExp();
			// TODO : 레벨업마다 진화, 기술 체크, 레벨업에서 종족값도 변해야함
		}
		// 스탯 재정산
		pokemonStat = GetStat();
	}
}
