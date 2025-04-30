using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonManagerS : Singleton<PokemonManagerS>
{
	public static PokemonManagerS Get;

	// 내파티
	public List<PokemonS> party = new List<PokemonS>();
	public Dictionary<int, PokemonStatS> GetBaseStat = new Dictionary<int, PokemonStatS>();

	void Awake()
	{
		if (Get == null)
		{
			Get = this;
			DontDestroyOnLoad(gameObject);
			PokemonBaseStatInit();

		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void Start()
	{
		party.Add(new PokemonS(1, "2", 1, new PokemonStatS(1, 2, 3, 4, 5, 6), new PokemonIVS(1, 2, 3, 4, 5, 6), PokeType.Fire, PokeType.Ice));
	}

	void PokemonBaseStatInit()
	{
		// 2세대 스타팅 포켓몬 등록
		GetBaseStat.Add(1, new PokemonStatS(45, 49, 65, 49, 65, 45)); // 치코리타
		GetBaseStat.Add(4, new PokemonStatS(39, 52, 43, 60, 50, 65)); // 브케인
		GetBaseStat.Add(7, new PokemonStatS(50, 65, 64, 44, 48, 43)); // 리아코
		//GetBaseStat.Add(10, new PokemonStatS(52, 60, 53, 50, 49, 55)); // 이상해씨
		//GetBaseStat.Add(12, new PokemonStatS(55, 65, 40, 50, 38, 41)); // 파이리
		//GetBaseStat.Add(15, new PokemonStatS(65, 42, 60, 49, 59, 30)); // 리자드
		//GetBaseStat.Add(19, new PokemonStatS(48, 59, 52, 52, 49, 55)); // 꼬부기
		//GetBaseStat.Add(20, new PokemonStatS(56, 52, 46, 56, 49, 44)); // 버터플
		//GetBaseStat.Add(22, new PokemonStatS(54, 51, 63, 47, 58, 52)); // 피죤
		//GetBaseStat.Add(25, new PokemonStatS(53, 50, 60, 49, 48, 49)); // 아보
		//GetBaseStat.Add(27, new PokemonStatS(48, 61, 52, 64, 41, 54)); // 피카츄
		//GetBaseStat.Add(31, new PokemonStatS(44, 60, 50, 51, 43, 39)); // 픽시
		//GetBaseStat.Add(33, new PokemonStatS(61, 41, 51, 34, 52, 24)); // 푸린
		//GetBaseStat.Add(37, new PokemonStatS(53, 51, 49, 42, 62, 55)); // 골뱃
		//GetBaseStat.Add(39, new PokemonStatS(58, 52, 62, 45, 61, 51)); // 고라파덕
	}
}
