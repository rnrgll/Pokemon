using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvent : PokeEvent
{
	[Header("대화 관련 설정")]
	[SerializeField] private Dialog dialog;
	[SerializeField] private GameObject npc;
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			if (Manager.Event.questEvent)
				return;



			NpcMover npcMover = npc.GetComponent<NpcMover>();
			Manager.Game.Player.AnimChange(Vector2.right);
			Manager.Game.Player.StopMoving();
			npcMover.AnimChange(Vector2.left);

			StartCoroutine(TriggerDialogue());
			
			Manager.Event.questEvent = true;
		}
	}
	public override void OnPokeEvent(GameObject player)
	{
		/*
		 여어 골드군, 기다리고 있었단다!
		오늘 너를 부른 것은 바탁이 있어서란다!
		내가 아는 사람중에
		포켓몬 할아버지라고 하는 
		이상한 것을 발견했을 때
		대발견! 이라고 떠드는 
		

		그리고 전에
		이번만큼은 진짜야!
		라는 메일이 왔단다.
		호기심이 생기지만 나도 조수도 
		포켓몬연구로 바빠서 말이다.
		네가 대신 가줬으면 좋겠구나

		물론 파트너가 될 포켓몬을 주겠다
		최근에 발견한 진귀한 포켓몬이란다
		자 고르거라!
		 */
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
