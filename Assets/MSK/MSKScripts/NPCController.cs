using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog dialog;

	private NPCAnimations NpcAnim;
	NpcCharacter npcCharacter;

	private void Awake()
	{
		npcCharacter = GetComponent<NpcCharacter>();
		NpcAnim = GetComponent<NPCAnimations>();
	}
	public void Interact()
    {
		//	if (DialogManager.Instance.isTyping == false) 
		//	{	StartCoroutine(DialogManager.Instance.ShowText(dialog));}
		StartCoroutine(npcCharacter.NpcMove(new Vector2(2, 0)));
	}
}
