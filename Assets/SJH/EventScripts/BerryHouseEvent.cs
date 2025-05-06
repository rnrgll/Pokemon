using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryHouseEvent : PokeEvent
{
	[Header("대화 관련 설정")]
	[SerializeField] private Dialog dialog;
	[SerializeField] private GameObject npc;
	public override void OnPokeEvent(GameObject player)
	{
		Debug.Log($"{Manager.Event.berryHouseEvent} 상태");
		if (Manager.Event.berryHouseEvent)
		{
			dialog = new Dialog(new List<string>
			{
				"나무열매를 조사하면",
				"나무열매가 떨어질꺼야"
			});
			Manager.Dialog.StartDialogue(dialog);
			return;
		}
		StartCoroutine(TriggerDialogue());
		// 나무열매 아이템을 주는 로직
		Manager.Event.berryHouseEvent = true;
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
