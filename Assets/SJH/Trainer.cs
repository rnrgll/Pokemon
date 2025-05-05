using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TrainerPokemon
{
	public string PokeName;
	public int PokeLevel;
}

[System.Serializable]
public class TrainerData
{
	public string Name;
	public bool IsFight;
	public int Money;
	public List<TrainerPokemon> TrainerPartyData;
	public GameObject TrainerObject;
}

public class Trainer : MonoBehaviour, IInteractable
{
	[SerializeField] public string trainerName;
	[SerializeField] public bool isFight;
	[SerializeField] public int money;
	[SerializeField] public List<TrainerPokemon> trainerPartyData;

	public void Interact(Vector2 position)
	{
		Debug.Log("트레이너 배틀 체크");
		if (isFight)
			return;

		if (trainerPartyData.Count <= 0)
			return;

		// 포켓몬 매니저 enemyParty에 파티 넣으면 배틀씬에서 트레이너 배틀로 인식
		Manager.Poke.enemyData = new TrainerData()
		{
			Name = trainerName,
			IsFight = isFight,
			Money = money,
			TrainerPartyData = trainerPartyData,
			TrainerObject = gameObject
		};

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
