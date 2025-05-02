using UnityEngine;

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

		//위치 비교
		if (position.y == npcPos.y)
		{
			if (npcPos.x - position.x == -1)
			{
				currentDirection = Vector2.left;
			}
			else { currentDirection = Vector2.right; }

		}
		AnimChange();
		if (Manager.Dialog.isTyping == false)
		{
			Manager.Dialog.StartDialogue(dialog);
		}
		Debug.Log($"{position} 위치");
		Debug.Log($"{npcPos} npc위치");

	}

	public void AnimChange()
	{
		anim.SetFloat("x", currentDirection.x);
		anim.SetFloat("y", currentDirection.y);
	}
}
