using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRoute30Event : PokeEvent
{
	[Header("대화 관련 설정")]
	[SerializeField] private Dialog dialog;
	[SerializeField] private GameObject npc;

	public override void OnPokeEvent(GameObject player)
	{
		//	이 이벤트는 
		if (Manager.Event.questEvent)
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

