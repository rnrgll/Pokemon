using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMover : MonoBehaviour
{
	//	NPC이동 루틴
	[SerializeField] List<Vector2> npcMovement;
	//	이동 거리
	[SerializeField] float npcSpeed = 2;
	//	이동 시간
	[SerializeField] float moveDirection;
	//	이동의 여부
	bool npcMoving;
	//	이동이 가능한지 여부
	bool isWalkAble;

	Animator anim;

	void NpcMoveStart()
	{
		//대화를 진행중인 Npc가 아니라면
		if (Manager.Dialog.isTyping != true)
		{
			npcMoving = true;
			NpcMoving();
		} 
		else
		{
			npcMoving = false;
		}
	}

	void NpcMoving()
	{
		//리스트 벡터2에 있는 값대로 움직이기
	}
	private bool IsWalkAble(Vector2 targetPos)
	{

		Vector2 inputDir = (targetPos - (Vector2)transform.position).normalized;
		RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + inputDir * 0.1f, inputDir, 1f);
		if (hit && (hit.transform.CompareTag("Wall") || hit.transform.CompareTag("NPC") || hit.transform.CompareTag("Player")))
		{
			return isWalkAble = false;
		}
		return isWalkAble;
	}
}
