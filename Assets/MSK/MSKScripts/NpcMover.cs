using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NpcMover : MonoBehaviour
{
	[SerializeField] List<Vector2> destinationPoints;
	[SerializeField] float moveDuration = 0.3f;

	bool npcMoving;
	int moveIndex = 0;
	Vector2 currentDirection;
	float moveSpeed;

	Animator anim;


	private Coroutine moveCoroutine;


	private void Awake()
	{
		moveSpeed = 1f / moveDuration;
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		if (Manager.Dialog.npcState != NpcState.Talking && !npcMoving)
		{
			moveCoroutine = StartCoroutine(MoveOneStep());
		}
	}

	//	사전 입력된 좌료로 이동
	private IEnumerator MoveOneStep()
	{
		npcMoving = true;

		Vector2 currentPos = (Vector2)transform.position;
		Vector2 targetPos = destinationPoints[moveIndex];
		currentDirection = (targetPos - currentPos).normalized;

		anim.SetFloat("x", currentDirection.x);
		anim.SetFloat("y", currentDirection.y);
		anim.SetBool("npcMoving", true);

		while (Vector2.Distance(transform.position, targetPos) > 0.1f)
		{
			// 이동 중에도 전방 장애물 감지
			if (!IsWalkAble(currentDirection))
			{
				// 애니메이션 멈추고 대기
				anim.SetBool("npcMoving", false);
				yield return null;
				continue;
			}
			else
			{
				// 다시 이동 시작
				anim.SetBool("npcMoving", true);
				transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
			}

			yield return null;
		}

		transform.position = targetPos;
		anim.SetBool("npcMoving", false);
		moveIndex = (moveIndex + 1) % destinationPoints.Count;

		yield return new WaitForSeconds(1f);
		npcMoving = false;

	}

	// NPC가 특정 방향으로 이동
	public void MoveTowardsDirection(Vector2 direction)
	{
		// 현재 위치에서 목표 위치(targetPos)
		Vector2 targetPos = (Vector2)transform.position + direction;

		// 애니메이션 방향 설정
		if (targetPos.x == transform.position.x)
		{
			anim.SetFloat("y", direction.y);
			anim.SetBool("npcMoving", true);
			transform.position = Vector2.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
		}
		else if (targetPos.y == transform.position.y)
		{			
			anim.SetFloat("x", direction.x);
			anim.SetBool("npcMoving", true);
			transform.position = Vector2.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
		}
	}

	//	코루틴 정지
	public void StopMoving()
	{
		if (moveCoroutine != null)
		{
			StopCoroutine(moveCoroutine);
			moveCoroutine = null;
		}
		npcMoving = false;
		anim.SetBool("npcMoving", false);

		Manager.Dialog.npcState = NpcState.Idle;  // 멈출 때 Idle 상태로 변경
	}

	private bool IsWalkAble(Vector2 currentDirection)
	{
		Vector2 startPos;
		startPos.x = transform.position.x;
		startPos.y = transform.position.y - 0.1f;
		RaycastHit2D hit = Physics2D.Raycast(startPos + currentDirection * 1.1f, currentDirection, 1f);

		// 자기 자신 무시
		if (hit.collider != null && hit.collider.transform.root == transform.root)
		{
			return true;
		}

		if (hit.collider != null)
		{
			Debug.Log($"{hit.transform.name}에 명중");
			string hitTag = hit.transform.tag;
			if (hitTag == "Wall" || hitTag == "NPC" || hitTag == "Player")
			{
				Debug.Log($"{hitTag} 이동 불가");
				return false;
			}
		}
		else
		{
			Debug.Log("명중 없음");
		}

		return true;
	}
}
