using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherrygroveCityInfoEvent : PokeEvent
{
	[Header("대화 관련 설정")]
	[SerializeField] private Dialog dialog;
	[SerializeField] private GameObject npc;
	Vector2 prevPos;
	NpcMover npcMover;
	private void Start()
	{
		prevPos = gameObject.transform.position;
		npcMover = GetComponent<NpcMover>();
	}

	public override void OnPokeEvent(GameObject player)
	{
		Debug.Log($"{Manager.Event.cherrygroveCityInfoEvent} 상태");

		if (!Manager.Event.cherrygroveCityInfoEvent)
		{
			dialog = new Dialog(new List<string>
			{
				"너 신출내기 트레이너지?",
				"급소를 찔렸지!",
				"누구나 처음은 있는 법",
				"여러가지 가르쳐줄게! ",
				"포켓몬 센터는",
				"상처입은 포켓몬을 맡기면",
				"지금부터 이후에 몇번이고",
				"신세를 지게 되겠지",
				"기억해 두는 것이 좋을 것이다!",
				"프렌들리 숍에서는",
				"포켓몬을 잡을 볼이라든지",
				"여러가지 품목을 팔고 있단다!",

				"이길의 끝은 30번 도로!",
				"모두 자랑하는 포켓몬을",
				"시합시키고 있단다!",

				"해안가 바위 옆으로 ",

				"가게된다면 바다란다!",
				"바다 속 에서는",
				" 밖에 없는 포켓몬도 있지!"
			});
			StartCoroutine(TriggerDialogue());
			return;
		}

		Manager.Event.cherrygroveCityInfoEvent = false;
	}
	private IEnumerator TriggerDialogue()
	{
		Manager.Dialog.StartDialogue(dialog);

		while (Manager.Dialog.isTyping)
		{
			yield return null;
		}
	}


	/*
			너 신출내기 트레이너지?
		급소를 찔렸지!
		좋아 좋아
		누구나 처음은 있는 법
		괜찮다면 내가 
		여러가지 가르쳐줄까?

		예 아니오

		그런가...
		내 부탁이었는데
		뭐 좋다싶은 생각이 들면 오너라



		좋아! 확실하게 안내해주마!

		포켓몬 센터 문 한칸 앞으로 이동

		이곳은 포켓몬 센터 
		상처입은 포켓몬을 맡기면
		눈 깜짝할 사이에 치료해준단다!
		지금부터 이후에 몇번이고
		신세를 지게 되겠지
		기억해 두는 것이 좋을 것이다!

		상점 문 한칸 앞으로 이동
		
		이곳은 프렌들리 숍
		포켓몬을 잡을 볼이라든지
		여러가지 품목을 팔고 있단다!

		도로 앞으로 이동

		이 끝은 30번 도로!
		모두 자랑하는 포켓몬을
		시합시키고 있단다!

		해안가 바위 옆으로 이동

		이 곳은 보는 바와 같이 바다!
		바다 속 밖에 없는 포켓몬도 있지!

		항아버지 집 앞으로 이동

		이곳은...
		내 집이란다
		상대해줘서 고맙다
		이거 답례로 주마
		
		골드는 맵 카드를 얻었다!

		골드의 포켓기어로 
		지도를 볼 수 있게 되었다!

		포켓기어는 편리카드를 끼우면
		점점 편리해진단다
		그럼 힘내거라!
		
		할아버지 집 안으로 이동하고 이벤트 종료

		 */
}
