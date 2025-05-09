using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TownExitEvent : PokeEvent
{

	[Header("대화 관련 설정")]
	[SerializeField] Dialog dialog;
	[SerializeField] GameObject npc;
	[SerializeField] bool isMove;
	[SerializeField] GameObject player;
	[SerializeField] private Vector2 originalNpcPosition;
	[SerializeField] NpcMover npcMover;

	void Start()
	{
		player = Manager.Game.Player.gameObject;
		npcMover = npc.GetComponent<NpcMover>();
	}

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
			if (Manager.Poke.party.Count <= 0)
			{
				Debug.Log("포켓몬을 가지고 있지 않습니다 삡");
				Player player = collision.gameObject.GetComponent<Player>();
				player.State = Define.PlayerState.Dialog;
				if (player.moveCoroutine != null)
					player.StopCoroutine(player.moveCoroutine);
				player.StopMoving();
				player.AnimChange();
				if (player.transform.position.y == -12)
				{
					npcMover.destinationPoints = new List<Vector2> { new Vector2(-20, -12)};
					Manager.Game.Player.transform.position = new Vector2(-20, -12);
						
				}
				else
				{
					npcMover.destinationPoints = new List<Vector2> { new Vector2(-22, -12) };
					Manager.Game.Player.transform.position = new Vector2(-20, -12);
				}
				
					StartCoroutine(TriggerDialogue());
				
				return;
			}
			
			Manager.Event.townExitEvent = true;
		}
	}

	void OnTriggerEnter2D(Collider2D player)
	{
		if (player.gameObject.CompareTag("Player"))
		{
			if (Manager.Event.townExitEvent)
				return;
			Debug.Log("플레이어 닿음");
			if (Manager.Poke.party.Count <= 0)
			{
				Debug.Log("포켓몬을 가지고 있지 않습니다 삡");
				if (player.transform.position.y == -12)
				{
					npcMover.destinationPoints = new List<Vector2> { new Vector2(-20, -12) };
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
		//if (Manager.Event.townExitEvent)
		//	return;
		//Debug.Log("플레이어 닿음");
		//if (Manager.Poke.party.Count <= 0)
		//{
		//	Debug.Log("포켓몬을 가지고 있지 않습니다 삡");
		//	if (player.transform.position.y == -12)
		//	{
		//		npcMover.destinationPoints = new List<Vector2> { new Vector2(-20, -12) };
		//	}
		//	else
		//	{
		//		npcMover.destinationPoints = new List<Vector2> { new Vector2(-22, -12) };
		//	}
		//	if (!isMove)
		//	{
		//		isMove = true;
		//		npcMover.isNPCMoveCheck = true;
		//		StartCoroutine(TriggerDialogue());
		//	}
		//	return;
		//}

		//Manager.Event.townExitEvent = true;
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
		npcMover = npc.GetComponent<NpcMover>();
		Manager.Dialog.StartDialogue(dialog);
		Manager.Game.Player.StopMoving();

		while (Manager.Dialog.isTyping)
		{
			yield return null;
		}
		isMove = false;
		Manager.Dialog.CloseDialog += ReturnNPCPlayer;
	}

	private IEnumerator ReturnNpcPosition()
	{
		Debug.Log("엔피시 이전위치로");
		npcMover = npc.GetComponent<NpcMover>();

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

	public void ReturnNPCPlayer()
	{
		Manager.Game.Player.transform.position = new Vector3(-18, -12);
		npc.transform.position = new Vector3(-12, -12);
		Manager.Dialog.CloseDialog -= ReturnNPCPlayer;
	}
}
