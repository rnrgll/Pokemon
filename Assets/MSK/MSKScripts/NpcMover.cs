using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMover : MonoBehaviour
{
	[SerializeField] List<Vector2> destinationPoints;
	[SerializeField] float moveDuration = 0.3f;

	bool npcMoving;
	int moveIndex = 0;
	Vector2 currentDirection;
	float moveSpeed;

	Animator anim;

	private void Awake()
	{
		moveSpeed = 1f / moveDuration;
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		if (!Manager.Dialog.isTyping && !npcMoving)
		{
			StartCoroutine(MoveOneStep());
		}
	}

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

	private bool IsWalkAble(Vector2 currentDirection)
	{
		Vector2 startPos;
		startPos.x = transform.position.x;
		startPos.y = transform.position.y - 0.5f;
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
