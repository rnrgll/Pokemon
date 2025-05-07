using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterSubEvent : PokeEvent
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
		if (collision.gameObject.CompareTag("Player") && Manager.Event.starterEvent)
		{
			if (Manager.Event.starterSubEvent)
				return;


			NpcMover npcMover = npc.GetComponent<NpcMover>();
			if(Manager.Game.Player.transform.position.x == -12)
			{
				npcMover.destinationPoints = new List<Vector2> { new Vector2(-12, 2) };
			}
			else
			{
				npcMover.destinationPoints = new List<Vector2> { new Vector2(-10, 2) };
			}

			npcMover.AnimChange(Vector2.right);
			originalNpcPosition = npc.transform.position;
			Manager.Game.Player.StopMoving();
			Debug.Log("플레이어 닿음");

			if (!isMove)
			{
				isMove = true;
				npcMover.isNPCMoveCheck = true;
				npcMover.AnimChange(Vector2.up);
				StartCoroutine(TriggerDialogue());
			}
			Manager.Event.starterSubEvent = true;
		}
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
			npcMover.destinationPoints = new List<Vector2> { new Vector2(-16, 2)};
			npcMover.AnimChange(Vector2.left);
			npcMover.moveIndex = 0;
			npcMover.isNPCMoveCheck = true;
		}

		while (npcMover.isNPCMoveCheck)
		{
			yield return null;
		}
	}
	public override void OnPokeEvent(GameObject player)
	{
		/*
	x -12인 경우 -10인 경우 체크


	직원npc : player.name군!
	직원npc : 심부름을 해주는 그대에게
	직원npc : 이것을 줄께요!
	player.name은 상처약(item)을 얻었다
	player.name은 상처약(item)을
	도구포켓에 넣었다
	둘밖에 없으니까
	약간의 일로도 당황스럽다....
	player : 아ㅡㅡ 바빠요
		 */
	}
}
