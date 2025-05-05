using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TrainerParty
{
	public string PokeName;
	public int PokeLevel;
}

public class Trainer : MonoBehaviour, IInteractable
{
	// 배틀했었는지 체크, 지더라도 true 안됨
	[SerializeField] public static bool isFight;
	[SerializeField] public List<TrainerParty> trainerPartyData;
	// 트레이너 파티
	[SerializeField] public List<Pokémon> trainerParty = new();

	void Start()
	{
		if (trainerPartyData.Count <= 0)
			return;

		foreach (var data in trainerPartyData)
		{
			if (string.IsNullOrEmpty(data.PokeName) || data.PokeLevel <= 0)
				continue;
			Pokémon poke = Manager.Poke.AddEnemyPokemon(data.PokeName, data.PokeLevel);
			trainerParty.Add(poke);
		}
	}

	public void Interact(Vector2 position)
	{
		// Test : 배틀
		Debug.Log("트레이너 배틀 체크");
		if (isFight)
			return;

		if (trainerParty.Count <= 0)
			return;

		// 포켓몬 매니저 enemyParty에 파티 넣으면 배틀씬에서 트레이너 배틀로 인식
		Manager.Poke.enemyParty = trainerParty;

		// 플레이어 스탑
		var player = Manager.Game.Player;
		player.GetComponent<Player>().StopMoving();

		// 씬전환 전 정보 저장
		player.PrevSceneName = SceneManager.GetActiveScene().name;

		Debug.Log("트레이너 배틀 시작");

		// 씬전환
		player.CurSceneName = "BattleScene";
		SceneManager.LoadScene("BattleScene");
	}
}
