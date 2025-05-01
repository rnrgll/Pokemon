using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
	public static Dictionary<string, string> sceneDic = new Dictionary<string, string>()
	{
		["주인공집2층"] = "PlayerHouse2F",
		["주인공집1층"] = "PlayerHouse1F",
		["연두마을"] = "NewBarkTown",
		["연구소"] = "ProfessorLab",
		["29번도로"] = "Route29",

	};

	public enum PortalType
	{
		Stair,      // 트리거 방식으로 씬변경
		Foothold    // Stay 일때 특정 방향키 입력하면 씬변경
	};

	public enum PlayerState
	{
		Field,			// 필드
		SceneChange,    // 씬 이동중
		Battle,			// 배틀중
		UI,				// UI 활성화중
		Menu,			// Menu 활성화중
		Dialog			// 대화 활성화중
	};

	public enum PokeType // 타입
	{
		None,       // 없음
		Normal,     // 노말
		Fire,       // 불꽃
		Water,      // 물
		Grass,      // 풀
		Electric,   // 전기
		Ice,        // 얼음
		Fighting,   // 격투
		Poison,     // 독
		Ground,     // 땅
		Flying,     // 비행
		Psychic,    // 에스퍼
		Bug,        // 벌레
		Rock,       // 바위
		Ghost,      // 고스트
		Dragon,     // 드래곤
		Dark,       // 악
		Steel,      // 강철
	}


	#region UI
	public enum UIInputType
	{
		Select,
		Cancel,
	}
	

	#endregion


	#region Item

	public int ItemMaxCnt = 99; //보유 가능한 최대 개수
	
	public enum ItemCategory // 아이템 카테고리(대분류)
	{ 
		Item, 
		Ball,
		KeyItem,
		TM_HM,
		Count
	}
	
	public enum ItemTarget
	{
		None,              // 사용 대상 없음
		Player,            // 플레이어 본인 대상
		MyPokemon,        // 내 포켓몬 대상
		EmneyPokemon,   // 상대 포켓몬 대상
	}

	public enum ItemUseContext //사용 환경
	{
		FieldOnly,
		BattleOnly,
		Both
	}

	#endregion
}
