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
		Debug.Log($"{Manager.Event.berryHouseEvent} 상태");
		if (Manager.Event.berryHouseEvent)
		{
			return;
		}
		StartCoroutine(TriggerDialogue());
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
