using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeQuestEvent : PokeEvent
{
	Vector2 targetPos1 = new Vector2(0, 4);  // 목표 위치
	Vector2 targetPos2 = new Vector2(0, 3);  // 목표 위치
	Player player;

	private void Awake()
	{
		player = FindObjectOfType<Player>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && !Manager.Event.pokegearEvent)
		{
			if (Manager.Event.beforeQuestEvent)
				return;

			Manager.Game.Player.State = Define.PlayerState.Dialog;
			Debug.Log("퀘스트 트리거 발생!!!!!!!!!!!");
			if(Manager.Game.Player.transform.position.x == -10)
				Manager.Game.Player.PlayerMove(targetPos2);
			else
				Manager.Game.Player.PlayerMove(targetPos1);

			Manager.Game.Player.StopMoving();
			Manager.Event.beforeQuestEvent = true;
		}
	}
	public override void OnPokeEvent(GameObject player)
	{
		Debug.Log("퀘스트 이벤트 실행");
	}
}
