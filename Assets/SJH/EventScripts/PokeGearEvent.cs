using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeGearEvent : PokeEvent
{
	[Header("대화 관련 설정")]
	[SerializeField] private Dialog dialog;
	[SerializeField] private GameObject npc;
	[SerializeField] private bool isMove;
	NpcMover npcMover;
	private Vector2 originalNpcPosition;

	private void ReturnNpcDialogue()
	{
		Manager.Dialog.CloseDialog -= ReturnNpcDialogue;
		StartCoroutine(ReturnNpcPosition());
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			if (Manager.Event.pokegearEvent)
				return;
			NpcMover npcMover = npc.GetComponent<NpcMover>();
			originalNpcPosition = npc.transform.position;
			Debug.Log("플레이어 닿음");

			if (!isMove)
			{
				isMove = true;
				npcMover.isNPCMoveCheck = true;
				StartCoroutine(TriggerDialogue());
			}
			Manager.Event.pokegearEvent = true;
		}
	}

	public override void OnPokeEvent(GameObject player)
	{
		Debug.Log("포켓기어 이벤트 실행!");
	}

	private IEnumerator TriggerDialogue()
	{
		Manager.Dialog.StartDialogue(dialog);
		Manager.Dialog.CloseDialog += ReturnNpcDialogue;

		while (Manager.Dialog.isTyping)
		{
			yield return null;
		}
		isMove = false;
	}

	private IEnumerator ReturnNpcPosition()
	{
		NpcMover npcMover = npc.GetComponent<NpcMover>();

		npcMover.StopMoving();
		if (npcMover.destinationPoints.Count == 0 || (Vector2)npc.transform.position != originalNpcPosition)
		{
			npcMover.destinationPoints = new List<Vector2> { new Vector2(8, 2), new Vector2(4, 2) };
			npcMover.moveIndex = 0;
			npcMover.isNPCMoveCheck = true;
		}

		while (npcMover.isNPCMoveCheck)
		{
			yield return null;
		}
	}
}
