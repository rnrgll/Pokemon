using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalEvent1 : PokeEvent
{
	[Header("대화 관련 설정")]
	[SerializeField] private Dialog dialog;
	[SerializeField] private GameObject npc;
	Vector2 prevPos;

	private void Start()
	{
		prevPos = gameObject.transform.position;
	}

	public override void OnPokeEvent(GameObject _npc)
	{
		Debug.Log($"{Manager.Event.rivalEvent1} 상태");
		if (Manager.Event.rivalEvent1)
		{
			dialog = new Dialog(new List<string>
			{
				"......",
				"이곳이 유명한",
				"공박사 포켓몬 연구소...",
				"뭐야",
				"사람을 뚫어지게 쳐다보고",
				//"\r\n\r\n\t\t플레이어 라이벌 동시에 x축 -1칸\r\n\t\t플레이어만 y축 -3칸\r\n\t\t라이벌 x축 +1칸"
			});
			Manager.Dialog.StartDialogue(dialog);
			
			
			return;
		}
		
			StartCoroutine(PlayerHit(_npc));
		
		
	}

	private IEnumerator PlayerHit(GameObject _npc)
	{
		Manager.Dialog.StartDialogue(dialog);
		while (Manager.Dialog.isTyping)
		{
			yield return null; // new Wait
		}

		if (gameObject != null)
		{
			gameObject.transform.position += new Vector3(-2f, 0f, 0f);
		}
		Player player = FindObjectOfType<Player>();

		if (player != null)
		{
			StartCoroutine(MovePlayerLerp(player.gameObject, new Vector3(0, -8f, 0), 0.5f));
		}
		yield return new WaitForSeconds(1f);

		gameObject.transform.position = prevPos;
	}

	private IEnumerator MovePlayerLerp(GameObject player, Vector3 offset, float duration)
	{
		if (player == null) yield break;

		Vector3 startPos = player.transform.position;
		Vector3 targetPos = startPos + offset;
		float elapsed = 0f;

		while (elapsed < duration)
		{
			player.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
			elapsed += Time.deltaTime;
			yield return null;
		}

		player.transform.position = targetPos;
	}

}
