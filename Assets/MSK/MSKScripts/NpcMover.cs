using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NpcMover : MonoBehaviour
{
	// 도착할 위치 : 기본은 시작 위치
	[SerializeField] public Vector2 exitPos;
	// 이동할 위치 2 단위로
	[SerializeField] public List<Vector2> destinationPoints;
	// 이동 시간
	[SerializeField] float moveDuration = 0.3f;
	// 회전 시간
	public float rotateInterval = 2.0f;
	// 이동 시간
	[SerializeField] float moveSpeed;
	// 엔피시 이동 여부
	[SerializeField] bool npcMoving;

	// destinationPoints의 이동할 순서
	public int moveIndex = 0;

	//	방향
	[SerializeField] public Vector2 currentDirection;

	Animator anim;
	private readonly Vector2[] directions = new Vector2[]
	{
		Vector2.right,
		Vector2.down,
		Vector2.left,
		Vector2.up
	};
	Coroutine moveCoroutine;
	Coroutine npcMoveCoroutione;
	Coroutine npcTurnCoroutione;

	//	반복 이동
	[SerializeField] public bool isNpcIntervalCheak;
	[SerializeField] public bool isPaused = false;
	//	일회성 이동
	[SerializeField] public bool isNPCMoveCheck;
	//	회전
	[SerializeField] public bool isNPCTurnCheck;
	public event Action MoveFin;
	
	private void Awake()
	{
		moveSpeed = 1f / moveDuration;
		anim = GetComponent<Animator>();
		currentDirection = Vector2.down;
		anim.SetFloat("x", currentDirection.x);
		anim.SetFloat("y", currentDirection.y);
	}

	private void Update()
	{
		if (isNPCMoveCheck && npcMoveCoroutione == null)
		{
			npcMoveCoroutione = StartCoroutine(NPCMove());
		}
		else if (isNPCTurnCheck && npcTurnCoroutione == null)
		{
			npcTurnCoroutione = StartCoroutine(NPCTurn());
		}
		else if (isNpcIntervalCheak && moveCoroutine == null)
		{
			moveCoroutine = StartCoroutine(LoopMove());
		}
	}
	IEnumerator LoopMove()
	{
		while (isNpcIntervalCheak)
		{
			while (isPaused)
			{
				anim.SetBool("npcMoving", false);
				yield return null;
			}
			yield return StartCoroutine(SingleStepMove());
			yield return new WaitForSeconds(1f);
		}

		moveCoroutine = null;
	}

	IEnumerator SingleStepMove()
	{
		npcMoving = true;

		Vector2 currentPos = (Vector2)transform.position;
		Vector2 targetPos = destinationPoints[moveIndex];
		currentDirection = (targetPos - currentPos).normalized;

		anim.SetFloat("x", currentDirection.x);
		anim.SetFloat("y", currentDirection.y);
		anim.SetBool("npcMoving", true);

		while (Vector2.Distance(transform.position, targetPos) > 0.01f)
		{
			if (!IsWalkAble(currentDirection))
			{
				anim.SetBool("npcMoving", false);
				yield return null;
				continue;
			}

			transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
			yield return null;
		}

		transform.position = targetPos;
		anim.SetBool("npcMoving", false);

		if (destinationPoints.Count > 0)
			moveIndex = (moveIndex + 1) % destinationPoints.Count;

		npcMoving = false;
	}

	public IEnumerator MoveOneStep()
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
			if (!IsWalkAble(currentDirection))
			{
				anim.SetBool("npcMoving", false);
				yield return null;
				continue;
			}
			else
			{
				anim.SetBool("npcMoving", true);
				transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
			}

			yield return null;
		}

		transform.position = targetPos;
		anim.SetBool("npcMoving", false);
		if (destinationPoints.Count > 0)
			moveIndex = (moveIndex + 1) % destinationPoints.Count;

		yield return new WaitForSeconds(1f);
		npcMoving = false;
	}

	public IEnumerator NPCMove()
	{
		Debug.Log("엔피시 이동");

		if ((Vector2)transform.position == destinationPoints[destinationPoints.Count - 1])
		{
			Debug.Log("엔피시 도착 이동 종료");
			isNPCMoveCheck = false;
			anim.SetBool("npcMoving", false);
			yield break;
		}

		if (destinationPoints.Count >= 1)
		{
			Debug.Log("엔피시 이동 시작");
			Vector3 startPos = transform.position;
			Vector3 targetPos = destinationPoints[moveIndex];
			currentDirection = (targetPos - startPos).normalized;

			Vector2 rayStartPos = (Vector2)startPos + Vector2.down * 0.3f;
			bool isBlocked = false;
			RaycastHit2D[] hits = Physics2D.RaycastAll(rayStartPos + currentDirection * 1.1f, currentDirection, 1f);

			foreach (RaycastHit2D hit in hits)
			{
				if (hit.collider != null && hit.transform.gameObject.CompareTag("Player"))
				{
					isBlocked = true;
					break;
				}
			}

			if (isBlocked)
			{
				anim.SetFloat("x", currentDirection.x);
				anim.SetFloat("y", currentDirection.y);
				anim.SetBool("npcMoving", false);
				yield return new WaitForSeconds(0.1f);
				yield break;
			}

			anim.SetFloat("x", currentDirection.x);
			anim.SetFloat("y", currentDirection.y);
			anim.SetBool("npcMoving", true);

			float time = 0;
			while (time < moveDuration)
			{
				time += Time.deltaTime;
				float percent = time / moveDuration;
				transform.position = Vector2.Lerp(startPos, targetPos, percent);
				yield return null;
			}
			transform.position = targetPos;

			if (moveIndex < destinationPoints.Count - 1)
				moveIndex++;
			else
			{
				isNPCMoveCheck = false;
				anim.SetBool("npcMoving", false);
				MoveFin?.Invoke();
			}

			npcMoveCoroutione = null;
		}
		else
		{
			yield return null;
		}
	}

	public void ListReverse()
	{
		if (destinationPoints != null && destinationPoints.Count > 0)
		{
			destinationPoints.Reverse();
		}
	}


	IEnumerator NPCTurn()
	{
		Debug.Log("엔피시 회전");

		int ran = UnityEngine.Random.Range(0, 4);
		currentDirection = directions[ran];

		anim.SetFloat("x", currentDirection.x);
		anim.SetFloat("y", currentDirection.y);

		yield return new WaitForSeconds(2f);
		npcTurnCoroutione = null;
	}

	public void MoveTowardsPosition(Vector2 targetPos)
	{
		currentDirection = (targetPos - (Vector2)transform.position).normalized;
		anim.SetFloat("x", currentDirection.x);
		anim.SetFloat("y", currentDirection.y);

		transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

		bool hasReached = Vector2.Distance(transform.position, targetPos) < 0.01f;
		anim.SetBool("npcMoving", !hasReached);
	}

	public void AnimChange(Vector2 position)
	{
		anim.SetFloat("x", position.x);
		anim.SetFloat("y", position.y);
	}

	public void StopMoving()
	{
		if (moveCoroutine != null)
		{
			StopCoroutine(moveCoroutine);
			moveCoroutine = null;
		}

		npcMoving = false;
		anim.SetBool("npcMoving", false);
		anim.SetFloat("x", currentDirection.x);
		anim.SetFloat("y", currentDirection.y);

		Manager.Dialog.npcState = NpcState.Idle;
		MoveFin?.Invoke();
	}

	private bool IsWalkAble(Vector2 currentDirection)
	{
		Vector2 startPos = new Vector2(transform.position.x, transform.position.y - 0.1f);
		RaycastHit2D hit = Physics2D.Raycast(startPos + currentDirection * 1.1f, currentDirection, 1f);

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
