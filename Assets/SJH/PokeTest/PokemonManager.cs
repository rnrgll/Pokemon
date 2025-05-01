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
		// Test용
		AddPokemon(1, 5);
	}

	public void AddPokemon(string pokeName, int level)
	{
		Pokémon pokemon = Instantiate(pokemonPrefab).GetComponent<Pokémon>();
		pokemon.Init(pokeName, level);

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
		pokemon.gameObject.name = pokemon.pokeName;
		if (party.Count >= 6)
		{
			Debug.Log($"가지고 있는 포켓몬 초과 PC로");
			pc.Add(pokemon);
			return;
		}

		party.Add(pokemon);
		Debug.Log($"골드은/는 {pokemon.pokeName} 을/를 얻었다!");
	}
}
