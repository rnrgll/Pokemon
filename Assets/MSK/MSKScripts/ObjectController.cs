using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectController : MonoBehaviour, IInteractable
{
	[SerializeField] Dialog dialog; // 대화 내용

	public void Interact(Vector2 position)
	{
		// 대화 중이 아니라면 대화를 시작
		if (Manager.Dialog.isTyping == false)
		{
			Debug.Log("오브제 상호작용");
			Manager.Dialog.StartDialogue(dialog);
		}
	}
}