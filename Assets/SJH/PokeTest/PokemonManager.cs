using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PokemonManager : Singleton<PokemonManager>
{
	public static PokemonManager Get => GetInstance();
	
	// 내파티
	public List<Pokémon> party = new List<Pokémon>();
	public Dictionary<int, PokemonStat> GetBaseStat = new Dictionary<int, PokemonStat>();
	public GameObject pokemonPrefab;

	void Start()
	{
		// 포켓몬 추가
		Pokémon pokemon1 = Instantiate(pokemonPrefab).GetComponent<Pokémon>();
		pokemon1.Init(1, 5);
		party.Add(pokemon1);

		Pokémon pokemon2 = Instantiate(pokemonPrefab).GetComponent<Pokémon>();
		pokemon2.Init("블레이범", 35);
		party.Add(pokemon2);
	}
}
