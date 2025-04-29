using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog dialog;

	private NPCAnimations NpcAnim;
	private void Awake()
	{
		NpcAnim = GetComponent<NPCAnimations>();
	}
	public void Interact()
    {
		if (DialogManager.Instance.isTyping == false) { 
        StartCoroutine(DialogManager.Instance.ShowText(dialog));
		}
	}
}
