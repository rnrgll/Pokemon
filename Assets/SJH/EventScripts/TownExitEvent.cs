using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownExitEvent : PokeEvent
{
	public override void OnPokeEvent(Collider2D player)
	{
		//@ 조건
		// 포켓몬을 들고 있지 않은 상태에서 마을밖으로 나갈때

		//@ 연관 엔피시
		// NPC2

		//@ 흐름
		// 플레이어 상태 바꾸기 UI
		// UI 활성화
		// 대사
		// 엔피시가 플레이어 앞까지 이동
		// 대사
		// 플레이어와 함께 엔피시 원래 위치로 이동
	}
}
