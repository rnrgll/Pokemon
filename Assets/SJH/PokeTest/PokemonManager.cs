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

	// 필드에 따라다니는 포켓몬
	public GameObject fieldPokemon;

	// 배틀을 할 트레이너 데이터
	public TrainerData enemyData;

	void Start()
	{
		//Test용 스타팅 포ㅓ켓몬 주기
		AddPokemon(4, 20);
		
		//====================테스트 코드===============//
		//메뉴 구현 중 테스트를 위한 임시 데이터 추가
		AddPokemon(1, 10);
		AddPokemon(8, 20);
		party[1].hp = 1;
		party[1].condition = StatusCondition.Poison;
		// AddPokemon(33, 10);
		
		enemyParty = new List<Pokémon>();
		enemyPokemon = Manager.Poke.AddEnemyPokemon("치코리타",15);
		
		AddPokemon("블레이범", 50);
		AddPokemon("피죤투",30);
		//===========================================//
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

	public Pokémon GetFirtstPokemon()
	{
		foreach (var poke in party)
		{
			if (!poke.isDead)
				return poke;
		}
		return null;
	}

	// 플레이어 파티 랭크 초기화
	public void PartyBattleStatInit()
	{
		foreach (var poke in party)
		{
			poke.pokemonBattleStack = new PokemonBattleStat(0);
		}
	}

	// 필드에 포켓몬 생성
	public void FieldPokemonInstantiate()
	{
		FieldPokemonDestroy();

		Pokémon poke = GetFirtstPokemon();

		if (poke == null)
			return;

		Follower follower = Manager.Data.SJH_PokemonData.GetFieldPokemon(poke.pokeName);
		Player player = Manager.Game.Player;

		if (follower == null)
			return;

		fieldPokemon = Instantiate(follower.gameObject, player.transform.position, Quaternion.identity);
	}

	public void FieldPokemonInstantiate(string pokeName)
	{
		FieldPokemonDestroy();

		Follower follower = Manager.Data.SJH_PokemonData.GetFieldPokemon(pokeName);
		Player player = Manager.Game.Player;

		if (follower == null)
			return;

		// 플레이어 오른쪽에 생성
		//Vector3 spawnPos = player.transform.position + Vector3.right * 2f;
		fieldPokemon = Instantiate(follower.gameObject, player.transform.position, Quaternion.identity);
	}

	public void FieldPokemonDestroy()
	{
		if (fieldPokemon != null)
		{
			Destroy(fieldPokemon);
		}
	}

	public void ClearPartyState()
	{
		foreach (var poke in party)
		{
			poke.StackReset();
		}
	}

	public bool PartyHeal()
	{
		// 포켓몬 모두 회복
		// 체력, 상태이상, 기술

		foreach(var poke in party)
		{
			// 체력
			poke.Heal(poke.maxHp);
			// 상태이상
			poke.condition = StatusCondition.Normal;
			// 기술
			for (int i = 0; i < poke.skillDatas.Count; i++)
			{
				var data = poke.skillDatas[i];
				data.IncreaseMaxPP();
				poke.skillDatas[i] = data;
			}
		}

		return true;
	}
}
