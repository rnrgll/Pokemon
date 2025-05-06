using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeGearEvent : PokeEvent
{
	[Tooltip("실행 됐는지 체크")]
	[SerializeField] bool isExecuted;

	[Header("대화 관련 설정")]
	[SerializeField] Dialog dialog;

	[SerializeField] GameObject npc;
	[SerializeField] bool isMove;

	private Vector2 originalNpcPosition;

	public List<Vector2> moves = new List<Vector2>();
	void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.CompareTag("Player"))
		{
			if (isExecuted)
				return;
			//	종료좌표 재설정
			originalNpcPosition = npc.transform.position;
			Debug.Log("플레이어 닿음");
			if (!isMove)
			{
				isMove = true;
				NpcMover npcMover = npc.GetComponent<NpcMover>();
				npcMover.isNPCMoveCheck = true;
				StartCoroutine(TriggerDialogue());
			}
			isExecuted = true;
		}
	}

	public override void OnPokeEvent(GameObject player)
	{
		// 포켓기어 이벤트 처리
		Debug.Log("포켓기어 이벤트 실행!");
	}

	private IEnumerator TriggerDialogue()
	{
		NpcMover npcMover = npc.GetComponent<NpcMover>();
		// 대화 처리
		Manager.Dialog.StartDialogue(dialog);

		while (Manager.Dialog.isTyping)
		{
			yield return null;
		}
		isMove = false;
	}
}
