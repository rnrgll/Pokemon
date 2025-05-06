using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownExitEvent : PokeEvent
{

	[Header("대화 관련 설정")]
	[SerializeField] Dialog dialog;
	[SerializeField] GameObject npc;
	[SerializeField] bool isMove;
	GameObject player;
	private Vector2 originalNpcPosition;
	NpcMover npcMover;

	private void ReturnNpcDialogue()
	{
		Manager.Dialog.CloseDialog -= ReturnNpcDialogue;
		StartCoroutine(ReturnNpcPosition());
	}

	void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.CompareTag("Player"))
		{
			if (Manager.Event.townExitEvent)
				return;
			Debug.Log("플레이어 닿음");
			if (Manager.Poke.party.Count < 0)
			{
				Debug.Log("포켓몬을 가지고 있지 않습니다 삡");
				if (player.transform.position.y == -12)
				{
					npcMover.destinationPoints = new List<Vector2> { new Vector2(-20, -12)};
				}
				else
				{
					npcMover.destinationPoints = new List<Vector2> { new Vector2(-22, -12) };
				}
				if (!isMove)
				{
					isMove = true;
					npcMover.isNPCMoveCheck = true;
					StartCoroutine(TriggerDialogue());
				}
				return;
			}
			
			Manager.Event.townExitEvent = true;
		}
	}
	public override void OnPokeEvent(GameObject player)
	{
		if (Manager.Poke.party.Count < 0)
		{
			Debug.Log("포켓몬을 가지고 있지 않습니다 삡");
			return;
		}
		Debug.Log("포켓몬을 가지고 있습니다 통과");
		// TODO : 포켓몬 없이 마을밖으로 나갈 때 엔피시가 가로막게

		// 장소 : 연두마을 -22, -12 or -22, -13
		// 엔피시 : NPC2
		// 조건 : 포켓몬을 들고 있지 않은 상태에서 마을밖으로 나갈때
		if (player.transform.position.y == -12)
		{

		}
		else
		{

		}
		/*

			아! 골드군
			혼자서 어디 가니?
			이런! 포켓몬도 지니지 않고
			도로에 나가다니 위험해!
			근처의 마을까지는
			야생의 포켓몬이 튀어 나오는
			풀숲만 있으니까

			(엔피시와 플레이어는 엔피시 원래 위치로 이동)

		 */
	}
	private IEnumerator TriggerDialogue()
	{
		NpcMover npcMover = npc.GetComponent<NpcMover>();
		Manager.Dialog.StartDialogue(dialog);

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
			npcMover.destinationPoints = new List<Vector2> { new Vector2(-12, -12) };
			npcMover.moveIndex = 0;
			npcMover.isNPCMoveCheck = true;
		}

		while (npcMover.isNPCMoveCheck)
		{
			yield return null;
		}
	}
}
