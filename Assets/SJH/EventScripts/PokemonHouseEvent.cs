using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonHouseEvent : PokeEvent
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

	private void NextNpcDialogue()
	{
		Manager.Dialog.CloseDialog -= NextNpcDialogue;
		StartCoroutine(NextNpcDialog());
	}
	private void LastNpcDialogue()
	{
		Manager.Dialog.CloseDialog -= LastNpcDialogue;
		StartCoroutine(LastNpcDialog());
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			if (Manager.Event.pokemonHouseEvent)
				return;


			Manager.Game.Player.PlayerMove(new Vector2(1, 1));
			Manager.Game.Player.AnimChange(Vector2.up);
			Manager.Game.Player.StopMoving();

			NpcMover npcMover = npc.GetComponent<NpcMover>();
			originalNpcPosition = npc.transform.position;
			Debug.Log("플레이어 닿음");

			if (!isMove)
			{
				isMove = true;
				npcMover.isNPCMoveCheck = true;
				StartCoroutine(TriggerDialogue());
			}
			Manager.Event.pokemonHouseEvent = true;
		}
	}


	public override void OnPokeEvent(GameObject player)
	{

		/*


(npc 옆 노란머리 말걸어옴)

(오박사)

npc할배박사
포켓몬 치료 이벤트

		 */
	}

	private IEnumerator LastNpcDialog()
	{
		Manager.Game.Player.AnimChange(Vector2.up);
		dialog = new Dialog(new List<string>
		{
				"공박사가 계신곳에 돌아갈 작정인가?",
				"약간 포켓몬을 쉬게 하는것이 좋겠지",
				"그럼 잘 부탁한다!"
		});
		Manager.Dialog.StartDialogue(dialog);

		while (Manager.Dialog.isTyping)
		{
			yield return null;
		}

	}
	private IEnumerator TriggerDialogue()
	{
		Manager.Dialog.StartDialogue(dialog);
		Manager.Dialog.CloseDialog += NextNpcDialogue;

		while (Manager.Dialog.isTyping)
		{
			yield return null;
		}
		isMove = false;
	}
	private IEnumerator NextNpcDialog()
	{
		Manager.Game.Player.AnimChange(Vector2.right);
		Manager.Game.Player.StopMoving();
		dialog = new Dialog(new List<string>
			{
				"호오! 네가 골드군인가!",
				"나는 오박사, 포켓몬 연구를 하고있단다",
				"오랜 친구인 포켓몬 할아버지를 방문 해보니까",
				"공박사가 있는 곳으로부터",
				"심부름군이 온다고 들어서",
				"기다리고 있었단다!",
				"호오! 진귀한 포켓몬을 가지고 있구나",
				"게다가..... 흐음 역시!",
				"공박사가 포켓몬을 줘서 ",
				"너에게 심부름을 부탁한 이유를 잘 알겠구나",
				"나와 공박사같은 연구가에게 있어서",
				"포켓몬은 소중한 친구이기 때문이란다",
				"너라면 포켓몬을 소중하게 키울 것 이라 생각한단다",
				"....그렇구나!",
				"너에게 기대를 걸고 나도 한가지 부탁을 할까!",
				"실은...... 이것이 포켓몬 도감이란다",
				"발견한 포켓몬의 데이터가 자동적으로 기록되어져",
				"페이지가 늘어간다고 하는 하이테크 도감이란다",
				"많은 포켓몬과 만나서 이 도감을 완전하게 만들어 주었으면 좋겠구나",
				"이런! 오랫동안 시간을 끌었구나",
				"이제부터 금빛시티에 가서 늘 그렇듯이 라디오 방송을 녹음하지 않으면 안 되겠구나",
				"그럼 골드군, 잘 부탁한다"
			});

		Manager.Dialog.StartDialogue(dialog);
		Manager.Dialog.CloseDialog += ReturnNpcDialogue;
		while (Manager.Dialog.isTyping)
		{
			yield return null;
		}
	}

	private IEnumerator ReturnNpcPosition()
	{
		NpcMover npcMover = npc.GetComponent<NpcMover>();

		if (npcMover.destinationPoints.Count == 0 || (Vector2)npc.transform.position != originalNpcPosition)
		{
			npcMover.destinationPoints = new List<Vector2> { new Vector2(2, -20), new Vector2(0, -20) };
			npcMover.moveIndex = 0;
			npcMover.isNPCMoveCheck = true;
		}

		npcMover.StopMoving();
		npcMover.MoveFin += LastNpcDialogue;
		while (npcMover.isNPCMoveCheck)
		{
			yield return null;
		}

		npc.SetActive(false);
	}
}

