using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterManager : Singleton<EncounterManager>
{
	public static EncounterManager Get => GetInstance();

	private string currentSceneName;
	private List<WildEncounterData> pool;

	void Start()
	{
		// 랜덤인카운터 이벤트 추가
		Player.OnGrassEntered += RandomEncounter;
	}

	void RandomEncounter()
	{
		Debug.Log("풀에 진입함 → 인카운트 판정 시작");
		string sceneName = SceneManager.GetActiveScene().name;

		if (currentSceneName != sceneName)
		{
			Debug.Log("씬 변경! 랜덤 인카운터 목록을 갱신합니다");
			currentSceneName = sceneName;
			pool = Manager.Data.EncounterData.GetDataByScene(currentSceneName);
		}

		// 스폰할 테이블 없음
		if (pool == null)
		{
			Debug.Log($"{currentSceneName} 에서는 스폰할 포켓몬이 없습니다!");
			return;
		}
		// 살아있는 포켓몬 없음
		if (!Manager.Poke.AlivePokemonCheck())
		{
			Debug.Log("배틀을 할 수 있는 포켓몬이 없습니다!");
			return;
		}

		// 15~25% 확률로 100% 테이블에서 인카운터
		int encounterRate = Random.Range(15, 26);
		int rate = Random.Range(0, 101);

		if (rate < encounterRate)
		{
			// 가중치
			// 확률 전부 더한 다음 (1 ~ 합산값 + 1) 에서 랜덤값을 뽑으면 랜덤확률
			int totalRate = 0;
			foreach (var ranPokeData in pool)
			{
				totalRate += ranPokeData.Rate;
			}

			int random = Random.Range(1, totalRate + 1);
			int target = 0;

			foreach (var ranPokeData in pool)
			{
				target += ranPokeData.Rate;
				if (random <= target)
				{
					int level = ranPokeData.GetRandomLevel();
					var pokeObject = Manager.Poke.AddEnemyPokemon(ranPokeData.Name, level);
					Debug.Log($"포켓몬 랜덤인카운터 : {ranPokeData.Name} Lv. {level} 이/가 나타났다!");

					// 포켓몬 매니저 enemyPokemon 에 값을 넣으면 야생 / enemyParty 에 값을 넣으면 트레이너
					Manager.Poke.enemyPokemon = pokeObject;

					// 애니메이션 종료
					var player = Manager.Game.Player;
					player.GetComponent<Player>().StopMoving();
					
					// 씬전환 전 정보 저장
					player.PrevSceneName = SceneManager.GetActiveScene().name;

					// 씬전환
					player.CurSceneName = "BattleScene_UIFix";
					SceneManager.LoadScene("BattleScene_UIFix");
					break;
				}
			}

		}
		else
		{
			// 인카운터 실패
			//Debug.Log($"실패 : {rate} >= {encounterRate}");
		}
	}
}