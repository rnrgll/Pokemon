using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private Player player;
	public Player Player => player;

	//게임 상황 판단용 플래그
	public bool IsInBattle { get; private set; }
	public bool IsWildBattle { get; private set; }
	public bool IsInDungeon { get; private set; }
	
	//적 대상 포켓몬 
	public Pokémon EnemyPokemon { get; private set; }
	
	//슬롯 타입
	public UI_PokemonParty.PartySlotType SlotType { get; private set; }
	
	
	//배틀 중 아이템 사용 여부 확인용 플래그
	public bool IsItemUsed { get; private set; }
	
	
	public void SetPlayer(Player player)
	{
		this.player = player;
	}

	public void ReleasePlayer()
	{
		player = null;
	}

	public void SetBattleState(bool isBattle, bool isWild = false, Pokémon enemyPokemon = null)
	{
		IsInBattle = isBattle;
		IsWildBattle = isWild;
		EnemyPokemon = enemyPokemon;
	}

	public void SetDungeonState(bool isInDungeon)
	{
		IsInDungeon = isInDungeon;
	}
	
	public void UpdateEnemyPokemon(Pokémon newEnemy)
	{
		EnemyPokemon = newEnemy;
	}

	public void EndBattle()
	{
		IsInBattle = false;
		IsWildBattle = false;
		EnemyPokemon = null;
	}

	public void SetSlotType(UI_PokemonParty.PartySlotType type)
	{
		SlotType = type;
	}

	public void SetIsItemUsed(bool isItemUsed) => IsItemUsed = isItemUsed;
}
