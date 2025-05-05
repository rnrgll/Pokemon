using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCController : MonoBehaviour, IInteractable
{
	[SerializeField] Dialog dialog;
	public Vector2 currentDirection = Vector2.down;
	private Vector2 npcPos;
	Define.NpcState state;
	Animator anim;

	private void Awake()
	{
		npcPos = this.transform.position;
		anim = GetComponent<Animator>();
	}
	public void Interact(Vector2 position)
	{

		AnimChange(position);
		if (Manager.Dialog.isTyping == false)
		{
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
