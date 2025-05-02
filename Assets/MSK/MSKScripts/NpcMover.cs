using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMover : MonoBehaviour
{
	//	NPC이동 루틴
	[SerializeField] NpcMovVector2 npcMov;

	//	이동 거리
	[SerializeField] float npcSpeed = 2;
	//	이동 시간
	[SerializeField] float moveDirection;
	//	이동의 여부
	bool npcMoving;
	//	이동이 가능한지 여부
	bool isWalkAble;
	//	NPC방향, 위치
	public Vector2 currentDirection;
	private Vector2 npcPos;

	Animator anim;
	private void Awake()
	{
		npcPos = this.transform.position;
		anim = GetComponent<Animator>();
	}
	private void Update()
	{
		NpcMoveStart();
	}
	public void NpcMoveStart()
	{
		//대화를 진행중인 Npc가 아니라면
		if (Manager.Dialog.isTyping != true)
		{
			npcMoving = true;
		//	NpcMoving(npcMov);
		} 
		else
		{
			IsWalkAble(currentDirection);
			npcMoving = false;
		}
	}

	//public IEnumerator NpcMoving(NpcMovVector2 vector)
	//{
		//리스트 벡터2에 있는 값대로 움직이기
		//리스트 카운트만큼 반복하기
		//리스트[0]에 있는 벡터대로 움직이기
		//리스트 카운트 ++
		//일정 시간 대기
	//}

	// npc 전방 확인
	private bool IsWalkAble(Vector2 targetPos)
	{

		RaycastHit2D hit = Physics2D.Raycast(npcPos, targetPos, 2f);
		if (hit && (hit.transform.CompareTag("Wall") || hit.transform.CompareTag("NPC") || hit.transform.CompareTag("Player")))
		{
			Debug.Log($"NPC 방향에 {hit.transform.tag} 있음");
			return isWalkAble = false;
		}else
		{ 
			Debug.Log("NPC 방향에 장애물 없음"); 
		}
		return isWalkAble;
	}
}
