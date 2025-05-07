using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterEvent : PokeEvent
{

	[SerializeField] private Dialog dialog;
	[SerializeField] Vector2 targetPos;  // 목표 위치


	public override void OnPokeEvent(GameObject player)
	{
		Debug.Log("스타팅 선택 시작!");
		if (Manager.Event.starterEvent) {
			Debug.Log("스타팅 선택 이미 진행됨!");
			dialog = new Dialog(new List<string>
			{
				"포켓몬이 들어 있는 몬스터 볼이다!"
			});

			StartCoroutine(TriggerDialogue());
			return;
		}


		Debug.Log("대화창!");
		dialog = new Dialog(new List<string>
		{
				"포켓몬 할아버지가 계신 곳은",
				"이마을 근처의",
				"무궁시티 끝이란다",
				"거기까지는 거의 외길이니까",
				"찾기 쉬울꺼다",
				"포켓몬이 상처를 입었다면",
				"저쪽에 있는 기계로",
				"포켓몬을 회복시켜주거라",
				"참 내 전화번호를",
				"알려줄테니까",
				"무슨 일이 있으면 연락하거라!",
				"골드는 공박사님의",
				"전화번호를 등록했다"
		});
		StartCoroutine(TriggerDialogue());
		Debug.Log("스타팅 종료!!");
		Manager.Event.starterEvent = true;
		/* 
		포켓몬 선택지 대사
		
		case
		불꽃의 포켓몬 브케인으로 하겠니!?
		물포켓몬 리아코가 마음에 드느냐!?
		풀포켓몬 치코리타가 마음에 들었느냐!?
		차분히 생각하여 정하는 것이 좋을꺼다
		소중한 파트너가 될테니까


		나도 이 녀석은 최고의 포켓몬이라고 생각한다!
		콜드는 공박사한테
		player.name은 공박사한테
		okemon.name을 받았다
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
