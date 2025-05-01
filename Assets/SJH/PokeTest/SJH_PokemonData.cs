using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SJH_PokemonData
{

	private Dictionary<int, SJH_PokemonData> dataById;
	private Dictionary<string, SJH_PokemonData> dataByName;

	// 도감번호
	public int Id;
	// 이름
	public string Name;
	// 종족값
	public PokemonStat BaseStat;
	// 타입1
	public Define.PokeType PokeType1;
	// 타입2
	public Define.PokeType PokeType2;
	// 경험치 타입
	public Define.ExpType ExpType;
	// 기술
	// 스프라이트1
	// 스프라이트2


	public void Init()
	{
		dataById = new Dictionary<int, SJH_PokemonData>()
		{
			[1] = new SJH_PokemonData
			{
				Id = 1,
				Name = "치코리타",
				BaseStat = new PokemonStat(45, 49, 65, 49, 65, 45),
				PokeType1 = Define.PokeType.Grass,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
			},
			[2] = new SJH_PokemonData
			{
				Id = 2,
				Name = "베이리프",
				BaseStat = new PokemonStat(60, 62, 80, 63, 80, 60),
				PokeType1 = Define.PokeType.Grass,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
			},
			[3] = new SJH_PokemonData
			{
				Id = 3,
				Name = "메가니움",
				BaseStat = new PokemonStat(80, 82, 100, 83, 100, 80),
				PokeType1 = Define.PokeType.Grass,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
			},
			[4] = new SJH_PokemonData
			{
				Id = 4,
				Name = "브케인",
				BaseStat = new PokemonStat(39, 52, 43, 60, 50, 65),
				PokeType1 = Define.PokeType.Fire,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
			},
			[5] = new SJH_PokemonData
			{
				Id = 5,
				Name = "마그케인",
				BaseStat = new PokemonStat(58, 64, 58, 80, 65, 80),
				PokeType1 = Define.PokeType.Fire,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
			},
			[6] = new SJH_PokemonData
			{
				Id = 6,
				Name = "블레이범",
				BaseStat = new PokemonStat(78, 84, 78, 109, 85, 100),
				PokeType1 = Define.PokeType.Fire,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
			},
			[7] = new SJH_PokemonData
			{
				Id = 7,
				Name = "리아코",
				BaseStat = new PokemonStat(50, 65, 64, 44, 48, 43),
				PokeType1 = Define.PokeType.Water,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
			},
			[8] = new SJH_PokemonData
			{
				Id = 8,
				Name = "엘리게이",
				BaseStat = new PokemonStat(65, 80, 80, 59, 63, 58),
				PokeType1 = Define.PokeType.Water,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
			},
			[9] = new SJH_PokemonData
			{
				Id = 9,
				Name = "장크로다일",
				BaseStat = new PokemonStat(85, 105, 100, 79, 83, 78),
				PokeType1 = Define.PokeType.Water,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
			},
			[10] = new SJH_PokemonData
			{
				Id = 10,
				Name = "구구",
				BaseStat = new PokemonStat(40, 45, 40, 35, 35, 56),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
			},
			[11] = new SJH_PokemonData
			{
				Id = 11,
				Name = "피죤",
				BaseStat = new PokemonStat(63, 60, 55, 50, 50, 71),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
			},
			[12] = new SJH_PokemonData
			{
				Id = 12,
				Name = "피죤투",
				BaseStat = new PokemonStat(83, 80, 75, 70, 70, 91),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
			},
			[13] = new SJH_PokemonData
			{
				Id = 13,
				Name = "깨비참",
				BaseStat = new PokemonStat(40, 60, 30, 31, 31, 70),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
			},
			[14] = new SJH_PokemonData
			{
				Id = 14,
				Name = "깨비드릴조",
				BaseStat = new PokemonStat(65, 90, 65, 61, 61, 100),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
			},
			[15] = new SJH_PokemonData
			{
				Id = 15,
				Name = "부우부",
				BaseStat = new PokemonStat(60, 30, 30, 36, 56, 50),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
			},
			[16] = new SJH_PokemonData
			{
				Id = 16,
				Name = "야부엉",
				BaseStat = new PokemonStat(100, 50, 50, 76, 96, 70),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
			},
			[17] = new SJH_PokemonData
			{
				Id = 17,
				Name = "꼬렛",
				BaseStat = new PokemonStat(30, 56, 35, 25, 35, 72),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumFast,
			},
			[18] = new SJH_PokemonData
			{
				Id = 18,
				Name = "레트라",
				BaseStat = new PokemonStat(55, 81, 60, 50, 70, 97),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumFast,
			},
			[19] = new SJH_PokemonData
			{
				Id = 19,
				Name = "꼬리선",
				BaseStat = new PokemonStat(35, 46, 34, 35, 45, 20),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumFast,
			},
			[20] = new SJH_PokemonData
			{
				Id = 20,
				Name = "다꼬리",
				BaseStat = new PokemonStat(85, 76, 64, 45, 55, 90),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumFast,
			},
			[24] = new SJH_PokemonData
			{
				Id = 24,
				Name = "캐터피",
				BaseStat = new PokemonStat(45, 30, 35, 20, 20, 45),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumFast,
			},
			[25] = new SJH_PokemonData
			{
				Id = 25,
				Name = "단데기",
				BaseStat = new PokemonStat(50, 20, 55, 25, 25, 30),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumFast,
			},
			[26] = new SJH_PokemonData
			{
				Id = 26,
				Name = "버터플",
				BaseStat = new PokemonStat(60, 45, 50, 80, 80, 70),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
			},
			[27] = new SJH_PokemonData
			{
				Id = 27,
				Name = "뿔충이",
				BaseStat = new PokemonStat(40, 35, 30, 20, 20, 50),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.MediumFast,
			},
			[28] = new SJH_PokemonData
			{
				Id = 28,
				Name = "딱충이",
				BaseStat = new PokemonStat(45, 25, 50, 25, 25, 35),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.MediumFast,
			},
			[29] = new SJH_PokemonData
			{
				Id = 29,
				Name = "독침붕",
				BaseStat = new PokemonStat(60, 80, 40, 45, 80, 75),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.MediumFast,
			},
			[32] = new SJH_PokemonData
			{
				Id = 32,
				Name = "페이검",
				BaseStat = new PokemonStat(40, 60, 40, 40, 40, 30),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.Fast,
			},
			[33] = new SJH_PokemonData
			{
				Id = 33,
				Name = "아리아도스",
				BaseStat = new PokemonStat(70, 90, 70, 60, 70, 40),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.Fast,
			},
			[34] = new SJH_PokemonData
			{
				Id = 34,
				Name = "꼬마돌",
				BaseStat = new PokemonStat(40, 80, 100, 30, 30, 20),
				PokeType1 = Define.PokeType.Rock,
				PokeType2 = Define.PokeType.Ground,
				ExpType = Define.ExpType.MediumSlow,
			},
			[35] = new SJH_PokemonData
			{
				Id = 35,
				Name = "데구리",
				BaseStat = new PokemonStat(55, 95, 115, 45, 45, 35),
				PokeType1 = Define.PokeType.Rock,
				PokeType2 = Define.PokeType.Ground,
				ExpType = Define.ExpType.MediumSlow,
			},
			[58] = new SJH_PokemonData
			{
				Id = 58,
				Name = "고오스",
				BaseStat = new PokemonStat(30, 35, 30, 100, 35, 80),
				PokeType1 = Define.PokeType.Ghost,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.MediumSlow,
			},
			[59] = new SJH_PokemonData
			{
				Id = 59,
				Name = "고우스트",
				BaseStat = new PokemonStat(45, 50, 45, 115, 55, 95),
				PokeType1 = Define.PokeType.Ghost,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.MediumSlow,
			},
			[64] = new SJH_PokemonData
			{
				Id = 64,
				Name = "모다피",
				BaseStat = new PokemonStat(50, 75, 35, 70, 30, 40),
				PokeType1 = Define.PokeType.Grass,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.MediumSlow,
			},
			[65] = new SJH_PokemonData
			{
				Id = 65,
				Name = "우츠동",
				BaseStat = new PokemonStat(65, 90, 50, 85, 45, 55),
				PokeType1 = Define.PokeType.Grass,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.MediumSlow,
			},
		};

		dataByName = new Dictionary<string, SJH_PokemonData>();
		foreach (var data in dataById.Values)
		{
			dataByName[data.Name] = data;
		};
	}

	public SJH_PokemonData GetPokemonData(int index)
	{
		var check = dataById[index];	
		if (check == null)
			return null;
		else
			return check;
	}

	public SJH_PokemonData GetPokemonData(string name)
	{
		var check = dataByName[name];
		if (check == null)
			return null;
		else
			return check;
	}
}
