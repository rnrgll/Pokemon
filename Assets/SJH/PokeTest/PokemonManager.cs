using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PokemonManager : Singleton<PokemonManager>
{
	public static PokemonManager Get => GetInstance();
	
	// 내파티
	public List<Pokémon> party = new List<Pokémon>();
	// PC
	public List<Pokémon> pc = new List<Pokémon>();
	// 포켓몬 프리펩
	public GameObject pokemonPrefab;

	// 배틀할 상대 포켓몬
	public Pokémon enemyPokemon;
	// 배틀할 상대 포켓몬들
	public List<Pokémon> enemyParty;

	void Start()
	{
		// Test용 스타팅 포ㅓ켓몬 주기
		AddPokemon(1, 5);
		
		//메뉴 구현 중 테스트를 위한 임시 데이터 추가
		AddPokemon(5, 10);
		AddPokemon(8, 20);
		party[1].hp = 1;
		party[1].condition = StatusCondition.Poison;
		// AddPokemon(33, 10);

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
	public Pokémon AddEnemyPokemon(string pokeName, int level)
	{
		Pokémon pokemon = Instantiate(pokemonPrefab).GetComponent<Pokémon>();
		pokemon.Init(pokeName, level);
		return pokemon;
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

	// 살아있는 포켓몬 체크용
	public bool AlivePokemonCheck()
	{
		foreach (var poke in party)
		{
			if (!poke.isDead && poke.hp > 0)
				return true;
		}
		return false;
	}

}
