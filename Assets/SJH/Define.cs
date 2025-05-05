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
		["도라지시티"] = "VioletCity",

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
	};

	public enum SkillType // 스킬 타입
	{
		Physical,   // 물리
		Special,    // 특수
		Status      // 특수기
	}

	public enum ExpType // 경험치 타입
	{
		Fast,       // 빠른 800,000			EXP = 4 * Level³ / 5
		MediumFast, // 약간 빠름 1,000,000	EXP = Level³
		MediumSlow, // 약간 느림 1,059,860	EXP = (6/5) * Level³ - 15 * Level² + 100 * Level - 140
	}

	public enum StatusCondition
	{
		Normal,
		Poison,			//독
		Burn,			//화상
		Freeze,			//얼음
		Sleep,			//잠듦
		Paralysis,		//마비 
		Confusion,		//혼란
		Faint,          //기절
		Flinch,			//풀죽음
	}


	#region UI
	public enum UIInputType
	{
		Up,
		Down,
		Left,
		Right,
		Select,
		Cancel
	}

	public static Dictionary<string, string> ColorCode = new Dictionary<string, string>()
	{
		{ "hp_red", "#F82805" }, { "hp_yellow", "#F8A81E" }, { "hp_green", "#02B821" }, {"bg_pink", "#F898F8"}, {"bg_green", "#A8F870"}, {"bg_blue","#88F8F8"}, {"bg_white","#F8F8F8"}
	};

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
		None,              // 사용 대상 없음 혹은 자동 결정
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


	#region Npc
	public enum NpcState
	{
		Idle,		//	대기
		Moving,     //	윰직임
		Talking,    //	대화중
	}
	#endregion
}
