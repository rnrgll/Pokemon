using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterPokemonObject : MonoBehaviour, IInteractable
{
	[SerializeField] Dialog dialog; // 대화 내용
	[SerializeField] PokeEvent pokeEvent;

	[SerializeField] static bool isGet;
	[SerializeField] string pokeName;
	private void Awake()
	{
		pokeEvent = GetComponent<PokeEvent>();
	}
	public void Interact(Vector2 position)
	{
		

		isGet = true;
		Manager.Poke.AddPokemon(pokeName, 5);

		// 필드에 포켓몬 생성
		//Manager.Poke.FieldPokemonInstantiate(pokeName);
		Manager.Poke.FieldPokemonInstantiate();

		// 대화 중이 아니라면 대화를 시작
		if (pokeEvent != null)
		{
			Debug.Log("이벤트 실행!!!!!!!!!!!!!!!!");
			pokeEvent.OnPokeEvent(gameObject);
		}
		else if (Manager.Dialog.isTyping == false)
		{
			Debug.Log("오브제 상호작용");
			Manager.Dialog.StartDialogue(dialog);
		}

		if (isGet)
		{
			Debug.Log($"이미 스타팅 포켓몬을 받아감");
			return;
		}
	}
}
