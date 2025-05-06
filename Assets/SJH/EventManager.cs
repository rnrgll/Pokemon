using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
	public static EventManager Get => GetInstance();

	// 트레이너 배틀 체크
	Dictionary<int, bool> trainerFightState;
	Dictionary<int, TrainerData> trainerData;

	void Start()
	{
		// 트레이너는 총 14명 필드11 체육관3
		trainerFightState = new();
		for (int i = 1; i <= 14; i++)
			trainerFightState[i] = false;

		// 이벤트매니저에서 필드의 트레이너들 관리
		trainerData = new()
		{
			// Route 30
			[1] = new TrainerData()
			{
				TrainerId = 1,
				Name = "반바지 꼬마 오성",
				IsFight = TrainerIsFight(1),
				Money = 64,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "꼬렛", PokeLevel = 4 },
				}
			},
			[2] = new TrainerData()
			{
				TrainerId = 2,
				Name = "반바지 꼬마 강철",
				IsFight = TrainerIsFight(2),
				Money = 64,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "구구", PokeLevel = 2 },
					new TrainerPokemon() { PokeName = "꼬렛", PokeLevel = 4 },
				}
			},
			[3] = new TrainerData()
			{
				TrainerId = 3,
				Name = "곤충채집소년 미키",
				IsFight = TrainerIsFight(3),
				Money = 48,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "캐터피", PokeLevel = 3 },
					new TrainerPokemon() { PokeName = "캐터피", PokeLevel = 3 },
				}
			},

			// Route 31
			[4] = new TrainerData()
			{
				TrainerId = 4,
				Name = "곤충채집소년 광일",
				IsFight = TrainerIsFight(4),
				Money = 32,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "캐터피", PokeLevel = 2 },
					new TrainerPokemon() { PokeName = "캐터피", PokeLevel = 2 },
					new TrainerPokemon() { PokeName = "뿔충이", PokeLevel = 3 },
					new TrainerPokemon() { PokeName = "캐터피", PokeLevel = 2 },
				}
			},

			// Sprout Tower
			// 1F
			[5] = new TrainerData()
			{
				TrainerId = 5,
				Name = "중 진연",
				IsFight = TrainerIsFight(5),
				Money = 96,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "모다피", PokeLevel = 3 },
					new TrainerPokemon() { PokeName = "모다피", PokeLevel = 3 },
					new TrainerPokemon() { PokeName = "모다피", PokeLevel = 3 },
				}
			},
			// 2F
			[6] = new TrainerData()
			{
				TrainerId = 6,
				Name = "중 묵연",
				IsFight = TrainerIsFight(6),
				Money = 96,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "모다피", PokeLevel = 3 },
					new TrainerPokemon() { PokeName = "모다피", PokeLevel = 3 },
					new TrainerPokemon() { PokeName = "모다피", PokeLevel = 3 },
				}
			},
			[7] = new TrainerData()
			{
				TrainerId = 7,
				Name = "중 영창",
				IsFight = TrainerIsFight(7),
				Money = 96,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "모다피", PokeLevel = 3 },
					new TrainerPokemon() { PokeName = "모다피", PokeLevel = 3 },
					new TrainerPokemon() { PokeName = "모다피", PokeLevel = 3 },
				}
			},
			// 3F
			[8] = new TrainerData()
			{
				TrainerId = 8,
				Name = "중 상연",
				IsFight = TrainerIsFight(8),
				Money = 192,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "모다피", PokeLevel = 6 },
				}
			},
			[9] = new TrainerData()
			{
				TrainerId = 9,
				Name = "중 가연",
				IsFight = TrainerIsFight(9),
				Money = 192,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "모다피", PokeLevel = 6 },
				}
			},
			[10] = new TrainerData()
			{
				TrainerId = 10,
				Name = "중 명연",
				IsFight = TrainerIsFight(10),
				Money = 224,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "모다피", PokeLevel = 7 },
					new TrainerPokemon() { PokeName = "부우부", PokeLevel = 7 },
				}
			},
			[11] = new TrainerData()
			{
				TrainerId = 11,
				Name = "중 청성",
				IsFight = TrainerIsFight(11),
				Money = 320,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "모다피", PokeLevel = 7 },
					new TrainerPokemon() { PokeName = "부우부", PokeLevel = 10 },
					new TrainerPokemon() { PokeName = "모다피", PokeLevel = 7 },
				}
			},
			// Violet City GYM
			[12] = new TrainerData()
			{
				TrainerId = 12,
				Name = "새조련사 소룡",
				IsFight = TrainerIsFight(12),
				Money = 216,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "깨비참", PokeLevel = 9 },
				}
			},
			[13] = new TrainerData()
			{
				TrainerId = 13,
				Name = "새조련사 날개",
				IsFight = TrainerIsFight(13),
				Money = 168,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "구구", PokeLevel = 7 },
					new TrainerPokemon() { PokeName = "구구", PokeLevel = 7 },
				}
			},
			[14] = new TrainerData()
			{
				TrainerId = 14,
				Name = "체육관 관장 비상",
				IsFight = TrainerIsFight(14),
				Money = 900,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "구구", PokeLevel = 7 },
					new TrainerPokemon() { PokeName = "피죤", PokeLevel = 9 },
				}
			},
		};
	}

	// 트레이너 IsFight = true 로 변경
	public void TrainerWin(int trainerId)
	{
		if (trainerFightState.ContainsKey(trainerId))
			trainerFightState[trainerId] = true;
	}

	// 트레이너가 싸울 수 있는지 체크
	public bool TrainerIsFight(int trainerId)
	{
		if (trainerFightState.ContainsKey(trainerId))
			return trainerFightState[trainerId];
		else
			return true;
	}


	public TrainerData GetTrainerDataById(int trainerId)
	{
		if (trainerData.ContainsKey(trainerId))
			return trainerData[trainerId];
		else
			return null;
	}

}
