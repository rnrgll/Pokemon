using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCController : MonoBehaviour, IInteractable
{
	[SerializeField] Dialog dialog; 
	
	private NpcMover npcMover;
	public Vector2 currentDirection = Vector2.down;
	private Vector2 npcPos;
	Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		npcMover = GetComponent<NpcMover>();
	}
	public void Interact(Vector2 position)
	{
		// Npc위치 현재 위치로 갱신

		npcPos = transform.position;
		AnimChange(position);

		if (npcMover != null)
		{
			npcMover.StopMoving();
		}

		anim.SetBool("npcMoving", false);
		if (Manager.Dialog.isTyping == false)
		{
			npcMover.npcState = Define.NpcState.Talking;
			Manager.Dialog.StartDialogue(dialog);
		}
	}


	//	NPC와 상호작용하는 방향 체크
	public void AnimChange(Vector2 position)
	{
		if (position.y == npcPos.y)
		{	//	좌우
			if (npcPos.x - position.x == -2)
				currentDirection = Vector2.right;
			else
				currentDirection = Vector2.left;
			anim.SetFloat("x", currentDirection.x);
			anim.SetFloat("y", 0);

		}
		else
		{	// 상하
			if (npcPos.y - position.y == -2)
				currentDirection = Vector2.up;
			else
				currentDirection = Vector2.down;
			anim.SetFloat("x", 0);
			anim.SetFloat("y", currentDirection.y);

		}
	}

}
