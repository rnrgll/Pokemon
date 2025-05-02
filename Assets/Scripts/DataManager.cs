using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
	// public PlayerData PlayerData { get; private set; }
	// // public InventoryData InventoryData { get; private set; }
	// // public PokemonPartyData PokemonPartyData { get; private set; }


	//플레이어 데이터 충돌 방지를 위해 이니셜 붙임(추후 수정 예정)
	public LDH_PlayerData LdhPlayerData { get; private set; }
	public SJH_PokemonData SJH_PokemonData { get; private set; }
	public DungeonMapData DungeonMapData { get; private set; }
	public EncounterData EncounterData { get; private set; }
	public SkillSData SkillSData { get; private set; }

	protected override void Init()
	{
		LdhPlayerData = new LDH_PlayerData();
		LdhPlayerData.Init();

		SJH_PokemonData = new SJH_PokemonData();
		SJH_PokemonData.Init();

		DungeonMapData = new DungeonMapData();
		DungeonMapData.Init();

		EncounterData = new EncounterData();
		EncounterData.Init();

		SkillSData = new SkillSData();
		SkillSData.Init();
	}
}
