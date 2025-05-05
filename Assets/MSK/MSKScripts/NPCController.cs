using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCController : MonoBehaviour, IInteractable
{
	[SerializeField] Dialog dialog; 
	
	private NpcMover npcMover;
	private Vector2 npcPos;
	public Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		npcMover = GetComponent<NpcMover>();
	}
	public void Interact(Vector2 position)
	{
		// Npc위치 현재 위치로 갱신

		npcPos = transform.position;
		npcMover.AnimChange(position);

		if (npcMover != null)
		{
			npcMover.npcTurn = false;
			npcMover.StopMoving();
		}

		anim.SetBool("npcMoving", false);
		if (Manager.Dialog.isTyping == false)
		{
			Manager.Dialog.npcState = Define.NpcState.Talking;
			Manager.Dialog.StartDialogue(dialog);
		}
	}

}
