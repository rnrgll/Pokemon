using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPokemonHas : PokeEvent
{
	[SerializeField] private Dialog dialog;
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			if (Manager.Event.starterEvent || !Manager.Event.questEvent)
				return;

			Manager.Game.Player.state = Define.PlayerState.Dialog;
			Debug.Log("퀘스트 트리거 발생!!!!!!!!!!!");
			Manager.Game.Player.AnimChange(Vector2.up);
			Manager.Game.Player.StopMoving();
			Manager.Game.Player.PlayerMove(Vector2.up);
			Manager.Game.Player.StopMoving();

			StartCoroutine(TriggerDialogue());
		}
	}
	/*
	 포켓몬 미분양 나가기 시도 시 
			(-12,8), (-10, 8)
			잠깐 잠깐 어디에 가느냐!
			플레이어 한 칸 위로
	 */
	public override void OnPokeEvent(GameObject player)
	{
		Debug.Log("퀘스트 이벤트 실행");
	}
	private IEnumerator TriggerDialogue()
	{
		Manager.Dialog.StartDialogue(dialog);
		while (Manager.Dialog.isTyping)
		{
			yield return null;
		}
	}
}
