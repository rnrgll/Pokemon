using UnityEngine;
using UnityEngine.UIElements;

public class NPCController : MonoBehaviour, IInteractable
{
	[SerializeField] Dialog dialog;

	[SerializeField] public Vector2 currentDirection;
	private Vector2 npcPos;
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
		Debug.Log($"{position} 위치");
		Debug.Log($"{npcPos} npc위치");

	}

	//	NPC와 상호작용하는 방향 체크
	public void AnimChange(Vector2 position)
	{
		if (position.y == npcPos.y)
		{
			if (npcPos.x - position.x == -2)
			{
				currentDirection = Vector2.right;
				Debug.Log("오른쪽");
			}
			else
			{
				currentDirection = Vector2.left;
				Debug.Log("왼쪽");
			}
			anim.SetFloat("x", currentDirection.x);
		}
		else
		{
			if (npcPos.y - position.y == -2)
			{
				Debug.Log("위");
				currentDirection = Vector2.up;
			}
			else
			{
				Debug.Log("아래");
				currentDirection = Vector2.down;
			}
			anim.SetFloat("y", currentDirection.y);
		}

	}

	
}
