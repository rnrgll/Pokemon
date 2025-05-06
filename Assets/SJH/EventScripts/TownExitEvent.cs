using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownExitEvent : PokeEvent
{
	[Tooltip("실행 됐는지 체크")]
	[SerializeField] bool isExecuted;

	[Header("대화 관련 설정")]
	[SerializeField] Dialog dialog;

	[SerializeField] GameObject npc;
	[SerializeField] bool isMove;

	void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.CompareTag("Player"))
		{
			if (isExecuted)
				return;
			//	종료좌표 재설정
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
			(플레이어 y가 -12인기 -13인지 체크)

			아! 골드군 (아무키입력대기)

			if (y == -12)
			(플레이어 → 방향 변경)
			(엔피시 플레이어 앞으로 이동) -20, -12
			(플레이어 엔피시 마주봄)

			if (y == -13)
			(플레이어 → 방향 변경
			(엔피시 플레이어 위에 타일로 이동) -22, -12
			(플레이어 엔피시 마주봄)

			혼자서 어디 가니? (아무키입력대기)

			(엔피시와 플레이어는 엔피시 원래 위치로 이동)

			이런! 포켓몬도 지니지 않고
			도로에 나가다니 위험해!

			근처의 마을까지는
			야생의 포켓몬이 튀어 나오는
			풀숲만 있으니까
		 */
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
