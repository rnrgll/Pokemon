using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeGearEvent : PokeEvent
{
	[Tooltip("실행 됐는지 체크")]
	[SerializeField] bool isExecuted;

	[SerializeField] GameObject npc;
	[SerializeField] static bool isMove;


	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Debug.Log("플레이어 닿음");
			if (isMove == false)
			{
				isMove = true;
				NpcMover npcMover = npc.GetComponent<NpcMover>();
				npcMover.isNPCMoveCheck = true;
			}
		}
	}

	public override void OnPokeEvent(GameObject player)
	{
		// 한번만 실행
		if (isExecuted)
			return;

		// TODO : 포켓기어 이벤트 실행

		// 장소 : 연두시티 - 플레이어 집 1층 8,5.5
		// 엔피시 : NPC1
		// 조건 : 플레이어 집 2층에서 1층으로 내려오자마자 실행
		/*
			(플레이어 State 변경)
			(플레이어 앞으로 엔피시가 와서 대사)
		 	아 골드!
			옆집의 공박사님이
			찾아왔었단다

			뭔지 너에게
			부탁할 것이 있다고 하셔서
			그래! 잊어먹을 뻔 했네

			수리를 보냈던
			포켓몬기어가 돌아왔단다
			여기!

			골드는(은)
			포켓몬 기어를(을)
			얻었다!

			포켓몬기어
			줄여서 포켓기어
			훌륭한 트레이너가
			되려면 가지고 있지 않으면 않될껄

			(요일은 스킵)

			(엔피시 원래 위치로 이동)
			(플레이어 state = field)
		 */

		isExecuted = true;
	}
}
