using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalEvent1 : PokeEvent
{
	[Header("대화 관련 설정")]
	[SerializeField] private Dialog dialog;
	[SerializeField] private GameObject npc;
	
	public override void OnPokeEvent(GameObject player)
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
			//if (gameObject != null)
			//{
			//	gameObject.transform.position += new Vector3(-2, 0);
			//}
			return;
		}
		if (player != null)
		{
			player.transform.position += new Vector3(0, -2f);
		}

		StartCoroutine(PlayerHit(player));

		//Rigidbody2D rigid = player.GetComponent<Rigidbody2D>();
		//if (player != null && rigid != null)
		//{
		//	rigid.AddForce(new Vector2(0, -30f));
		//}
	}

	private IEnumerator PlayerHit(GameObject player)
	{
		Manager.Dialog.StartDialogue(dialog);
		if (player != null)
		{
			player.transform.position += new Vector3(0, -4f);
		}
		while (Manager.Dialog.isTyping)
		{
			yield return null;
		}
	}

}
