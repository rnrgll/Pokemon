using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PokemonManager : Singleton<PokemonManager>
{
	public static PokemonManager Get => GetInstance();
	
	// 내파티
	public List<Pokémon> party = new List<Pokémon>();
	public List<Pokémon> pc = new List<Pokémon>();
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

	public void AddStartingPokemon(string name)
	{
		// 스타팅 얻는 함수
		Pokémon pokemon = Instantiate(pokemonPrefab).GetComponent<Pokémon>();
		pokemon.Init(name, 5);

		AddParty(pokemon);
	}

	public void AddPokemon(string name, int level)
	{
		Pokémon pokemon = Instantiate(pokemonPrefab).GetComponent<Pokémon>();
		pokemon.Init(name, level);

		AddParty(pokemon);
	}

	public void AddPokemon(int pokedex, int level)
	{
		Pokémon pokemon = Instantiate(pokemonPrefab).GetComponent<Pokémon>();
		pokemon.Init(pokedex, level);

		AddParty(pokemon);
	}

	private void AddParty(Pokémon pokemon)
	{
		if (party.Count >= 6)
		{
			Debug.Log($"가지고 있는 포켓몬 초과 PC로");
			pc.Add(pokemon);
			return;
		}
		party.Add(pokemon);
	}
}
