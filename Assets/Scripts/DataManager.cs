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
	public PlayerData PlayerData { get; private set; } //플레이어
	public SJH_PokemonData SJH_PokemonData { get; private set; } //포켓몬
	public DungeonMapData DungeonMapData { get; private set; } //던전 정보(연관 씬, 좌표)
	public EncounterData EncounterData { get; private set; } //몬스터 출현
	public SkillSData SkillSData { get; private set; } //스킬
	
	public ItemDatabase ItemDatabase { get; private set; } //아이템 데이터베이스

	protected override void Init()
	{
		PlayerData = new PlayerData();
		PlayerData.Init();

		SJH_PokemonData = new SJH_PokemonData();
		SJH_PokemonData.Init();

		DungeonMapData = new DungeonMapData();
		DungeonMapData.Init();

		EncounterData = new EncounterData();
		EncounterData.Init();

		SkillSData = new SkillSData();
		SkillSData.Init();

		ItemDatabase = new ItemDatabase();
		ItemDatabase.Init();
	}
}
