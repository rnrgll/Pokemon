using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMover : MonoBehaviour
{
	//	NPC이동 루틴
	[SerializeField] List<Vector2> movePattens;

	//	이동 거리
	[SerializeField] float npcSpeed = 2;
	[SerializeField] float moveDuration = 0.3f;
	//	이동의 여부
	bool npcMoving;
	//	이동이 가능한지 여부
	bool isWalkAble = true;
	//	npc움직임 리스트
	private int movList = 0;
	//	NPC방향, 위치
	public Vector2 currentDirection;

	Animator anim;

	private void Awake()
	{   // 초기 방향 아래
		currentDirection = Vector2.down;
		anim = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		NpcMoveStart();
	}

	public void NpcMoveStart()
	{
		//대화를 진행중인 Npc가 아니라면
		if (Manager.Dialog.isTyping != true)
		{
			StartCoroutine(MoveOneStep());
		}
	}

	private IEnumerator MoveOneStep()
	{
		npcMoving = true;

		//	움직일 방향, 시작점, 종점
		currentDirection = movePattens[movList];
		Vector2 startPos = transform.position;
		Vector2 targetPos = startPos + (currentDirection * npcSpeed);

		// 방향에 따라 애니메이션 방향 설정
		anim.SetFloat("x", currentDirection.x);
		anim.SetFloat("y", currentDirection.y);
		anim.SetBool("npcMoving", true);

		float time = 0;
		while (time < moveDuration && npcMoving)
		{
			time += Time.deltaTime;
			float percent = time / moveDuration;
			transform.position = Vector2.Lerp(startPos, targetPos, percent);
			yield return null;
		}
		transform.position = targetPos;

		//	움직임 재설정
		if (movList > movePattens.Count)
			movList = 0;
		else
			movList++;

		anim.SetBool("npcMoving", false);
		npcMoving = false;

		yield return new WaitForSeconds(0.2f);  // 움직임 사이 약간의 쉼
	}

	//public IEnumerator NpcMoving(NpcMovVector2 vector)
	//{
	//		리스트 벡터2에 있는 값대로 움직이기
	//		리스트 카운트만큼 반복하기
	//		리스트[0]에 있는 벡터대로 움직이기
	//		리스트 카운트 ++
	//		일정 시간 대기
	//}

	// npc 전방 확인
	//	이동이 가능한지 여부
	private bool IsWalkAble(Vector2 currentDirection)
	{
		//	Npc위치 + 방향에서 발사, 방향으로 1f만큼 발사
		Vector2 startPos;
		startPos.y = transform.position.y - 0.5f;
		startPos.x = transform.position.x;

		RaycastHit2D hit = Physics2D.Raycast(startPos + currentDirection * 1.1f, currentDirection, 1f);


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
	//	Raycast 방향
	public void DirChange()
	{
		//	if ("Npc가 위를 보면") currentDirection = Vector2.up;
		//	else if ("Npc가 아래를 보면") currentDirection = Vector2.down;
		//	else if ("Npc가 왼쪽을 보면") currentDirection = Vector2.left;
		//	else if ("Npc가 오른쪽을 보면") currentDirection = Vector2.right;
	}


}
