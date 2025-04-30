using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PokemonManager : Singleton<PokemonManager>
{
	public static PokemonManager Get;
	
	// 내파티
	public List<Pokémon> party = new List<Pokémon>();
	public Dictionary<int, PokemonStat> GetBaseStat = new Dictionary<int, PokemonStat>();
	public GameObject pokemonPrefab;

	void Awake()
	{
		if (Get == null)
		{
			Get = this;
			DontDestroyOnLoad(gameObject);
			PokemonBaseStatInit();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		var test = Instantiate(pokemonPrefab).GetComponent<Pokémon>();
		test.id = 1;
		party.Add(Instantiate(pokemonPrefab).GetComponent<Pokémon>());
	}

	void PokemonBaseStatInit()
	{
		// 2세대 스타팅 포켓몬 등록
		GetBaseStat.Add(1, new PokemonStat(45, 49, 65, 49, 65, 45)); // 치코리타
		GetBaseStat.Add(4, new PokemonStat(39, 52, 43, 60, 50, 65)); // 브케인
		GetBaseStat.Add(7, new PokemonStat(50, 65, 64, 44, 48, 43)); // 리아코
	}
}
