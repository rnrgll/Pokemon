using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EventManager : Singleton<EventManager>
{
	public static EventManager Get => GetInstance();

	// 트레이너 배틀 체크
	Dictionary<int, bool> trainerFightState;
	Dictionary<int, TrainerData> trainerData;

	// 이벤트 트리거들
	[SerializeField] public bool pokegearEvent;         // 2층에서 1층가면 포켓기어 설명 + 공박사한테 가라는 이벤트
	[SerializeField] public bool berryHouseEvent;          // 30번도로 나무열매집 엔피시 말걸면 나무열매 주는 이벤트

	[SerializeField] public bool questEvent;                // 포켓몬 할아버지 집 가라는 이벤트
	[SerializeField] public bool starterEvent;             // 공박사에게 스타팅 포켓몬 받는 이벤트
	[SerializeField] public bool beforeQuestEvent;			//	박사 연구소 첫 입장 시 강제 움직임 이벤트
	[SerializeField] public bool starterSubEvent;           // 스타팅 받고 나갈 때 조수가 상처약 주는 이벤트
	[SerializeField] public bool pokemonHouseEvent;        // 포켓몬 할아버지집 들어가면 강제 이벤트 (대충 설명)


	[SerializeField] public bool townExitEvent;             // 연두마을 나갈 때 포켓몬 없으면 못나가게하는 이벤트


	[SerializeField] public bool rivalEvent1;              // 연두마을 연구소 옆에서 라이벌한테 말걸면 차이는 이벤트
	[SerializeField] public bool eggEvent;                 // 체육관 승리 후 체육관 나가면 강제 이벤트 (전화) 센터가면 포켓몬알줌 끝
	[SerializeField] public bool gymEvent;                 // 체육관 이벤트 (승리시)
	//	[SerializeField] public bool teachEvent;           // 무궁시티 체육관 왼쪽에 말걸면 학교로 데려가서 설명해주는 이벤트
	[SerializeField] public bool cherrygroveCityInfoEvent; // 무궁시티 마을 입구 할아버지 말걸면 마을 소개해주는 이벤트
	[SerializeField] public bool sproutTowerEvent;         // 모다피탑 3층에서 라이벌과 스님이 얘기하는 이벤트



	//questEvent True일 경우 방생
	//questEvent True starterEvent true 일 경우 방생
	[SerializeField] public bool adventureEvent;           // 연구소 들어가면 모험 떠나라는 이벤트

	[SerializeField] public bool backNewBarkTownEvent;     // pokemonHouseEvent true일 떄 포켓몬 할아버지집 나가면 강제 이벤트 (대충 마을로오라는)
	[SerializeField] public bool rivalEvent2;              // 연두마을 가는길에 라이벌 배틀 이벤트

				[SerializeField] public bool teachEvent;               // 무궁시티 체육관 왼쪽에 말걸면 학교로 데려가서 설명해주는 이벤트
	 // 공박사 이벤트(adventureEvent) 가 true = 비활성화, false = 활성화로 영향을 받는 이벤트
	[SerializeField] public bool blockRoute30Event;        


	



	[SerializeField] public int playerStarter = 0;				// 플레이어가 선택한 스타팅 포켓몬 0 브케인, 1 리아코 2 치코리타

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
			// Rival
			[15] = new TrainerData()
			{
				TrainerId = 15,
				Name = "???",
				IsFight = TrainerIsFight(15),
				Money = 900,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = SetRivalPokemon(), PokeLevel = 5 },
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

	public string SetRivalPokemon()
	{
		// 플레이어가 선택한 스타팅 포켓몬 0 브케인, 1 리아코 2 치코리타
		switch (playerStarter)
		{
			case 0: return "리아코";
			case 1: return "치코리타";
			default: return "브케인";
		}
	}
}
