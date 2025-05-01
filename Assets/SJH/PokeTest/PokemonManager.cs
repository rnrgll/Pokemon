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

	void Start()
	{
		GameObject pokeObject = Instantiate(pokemonPrefab);
		Pokémon pokemon = pokeObject.GetComponent<Pokémon>();
		pokemon.Init(1, 5);
		party.Add(pokemon);
	}
}
