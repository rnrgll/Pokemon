using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfessorCallEvent : PokeEvent
{
	[Header("대화 관련 설정")]
	[SerializeField] private Dialog dialog;
	[SerializeField] private GameObject npc;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && Manager.Event.gymEvent && !Manager.Event.eggEvent)
		{
			Debug.Log("이벤트충돌");
			
			dialog = new Dialog(new List<string>
			{
			"크 큰일났구나!!",
			"에- 그러니까 뭐가 뭐였는지...",
			"어떡하지....",
			"아무튼 큰일이란다",
			"지금 바로 돌아오너라"
			});
			Manager.Dialog.StartDialogue(dialog);
			return;
		}
		StartCoroutine(PrintCor());
		Manager.Event.eggEvent = false;
	}

	private IEnumerator PrintCor()
	{
		Manager.Dialog.StartDialogue(dialog);

		while (Manager.Dialog.isTyping)
		{
			yield return null;
		}
	}
	public override void OnPokeEvent(GameObject player)
	{
		/*
		 npc공박사 전화옴
(공박사 전화1)
크 큰일났구나!!
에- 그러니까 뭐가 뭐였는지...
어떡하지....
아무튼 큰일이란다
지금 바로 돌아오너라
		 */
	}
}
