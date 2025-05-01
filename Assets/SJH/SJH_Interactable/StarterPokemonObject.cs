using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterPokemonObject : MonoBehaviour, IInteractable
{
	[SerializeField] static bool isGet;
	[SerializeField] string pokeName;

	public void Interact()
	{
		if (isGet)
		{
			Debug.Log($"이미 스타팅 포켓몬을 받아감");
			return;
		}

		isGet = true;
		Manager.Poke.AddPokemon(pokeName, 5);
	}
}
