using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterPokemonObject : MonoBehaviour, IInteractable
{
	[SerializeField] static bool isGet;
	[SerializeField] string pokeName;

	public void Interact(Vector2 position)
	{
		if (isGet)
		{
			Debug.Log($"이미 스타팅 포켓몬을 받아감");
			return;
		}

		isGet = true;
		Manager.Poke.AddPokemon(pokeName, 5);

		// TODO : 필드에 포켓몬 생성 x+2
		Follower follower = Manager.Data.SJH_PokemonData.GetFieldPokemon(pokeName);
		Player player = Manager.Game.Player;

		Vector3 spawnPos = player.transform.position + Vector3.right * 2f;
		Instantiate(follower, spawnPos, Quaternion.identity);
	}
}
