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
	private void Update()
	{
		IsWalkAble(currentDirection);
	}

	//	이동이 가능한지 여부
	bool isWalkAble;
	private bool IsWalkAble(Vector2 currentDirection)
	{
		//	Npc위치 + 방향에서 발사, 방향으로 1f만큼 발사

		RaycastHit2D hit = Physics2D.Raycast(npcPos + currentDirection* 1.1f, currentDirection, 1f);


		if (hit.collider != null)
		{
			Debug.Log($"{hit.transform.name}에 명중");
		}
		else
		{

			Debug.Log($"명중 없음");
		}



		//	hit.Tag 검사후 이동 가능 여부 판단
		if (hit.transform.gameObject.transform.tag == "Wall" || hit.transform.gameObject.transform.tag == "NPC" || hit.transform.gameObject.transform.tag == "Player")
		{
			Debug.Log($"{hit.transform.gameObject.transform.tag} 이동 불가");
			isWalkAble = false;
		}
		else
		{
			isWalkAble = true;
		}
		Debug.Log($"이동 가능 여부는 : {isWalkAble} 입니다.");

		return isWalkAble;
	}
}
