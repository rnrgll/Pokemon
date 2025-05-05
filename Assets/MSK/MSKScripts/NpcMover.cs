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

	Animator anim;

	private void Awake()
	{
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

		// 이동 가능 여부 확인
		if (!IsWalkAble(currentDirection))
		{
			Debug.Log("이동 불가, 다음 목적지로");
			moveIndex = (moveIndex + 1) % destinationPoints.Count;
			npcMoving = false;
			yield break;
		}

		// 애니메이션
		anim.SetFloat("x", currentDirection.x);
		anim.SetFloat("y", currentDirection.y);
		anim.SetBool("npcMoving", true);

		float time = 0f;
		while (time < moveDuration && npcMoving)
		{
			time += Time.deltaTime;
			float percent = time / moveDuration;
			transform.position = Vector2.Lerp(currentPos, targetPos, percent);
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
		startPos.y = transform.position.y - 0.5f; // 하단 보정 유지

		Debug.DrawRay(startPos + currentDirection * 1.1f, currentDirection, Color.red, 1f);

		RaycastHit2D hit = Physics2D.Raycast(startPos + currentDirection * 1.1f, currentDirection, 1f);

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
