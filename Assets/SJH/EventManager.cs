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
			[1] = new TrainerData()
			{
				TrainerId = 1,
				Name = "재훈",
				IsFight = TrainerIsFight(1),
				Money = 50,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "꼬렛", PokeLevel = 4 },
				}
			},

			[2] = new TrainerData()
			{
				TrainerId = 2,
				Name = "훈재",
				IsFight = TrainerIsFight(2),
				Money = 100,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "꼬렛", PokeLevel = 4 },
					new TrainerPokemon() { PokeName = "구구", PokeLevel = 4 },
				}
			},

			[3] = new TrainerData()
			{
				TrainerId = 3,
				Name = "훈재훈",
				IsFight = TrainerIsFight(3),
				Money = 500,
				TrainerPartyData = new List<TrainerPokemon>()
				{
					new TrainerPokemon() { PokeName = "뿔충이", PokeLevel = 4 },
					new TrainerPokemon() { PokeName = "캐터피", PokeLevel = 4 },
					new TrainerPokemon() { PokeName = "단데기", PokeLevel = 4 },
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
