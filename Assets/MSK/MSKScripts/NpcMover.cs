using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NpcMover : MonoBehaviour
{
	[SerializeField] List<Vector2> destinationPoints;
	[SerializeField] float moveDuration = 0.3f;
	
	

	public float rotateInterval = 2.0f;
	float moveSpeed;
	bool npcMoving;
	public bool npcTurn;
	private int directionIndex = 0;
	int moveIndex = 0;
	float timer;

	
	Vector2 currentDirection;
	private Vector2 npcPos;
	public Vector2 dir;
	
	
	Animator anim;
	NPCController npcController;
	private Coroutine moveCoroutine;
	private readonly Vector2[] directions = new Vector2[]
	{
		Vector2.right,
		Vector2.down,
		Vector2.left,
		Vector2.up
	};

	private void AutoRotate()
	{
		dir = directions[directionIndex];
		anim.SetFloat("x", dir.x);
		anim.SetFloat("y", dir.y);

		directionIndex = (directionIndex + 1) % directions.Length;
	}

	private void Awake()
	{
		npcPos = this.transform.position;
		moveSpeed = 1f / moveDuration;
		npcController = GetComponent<NPCController>();
		anim = npcController.anim;
	}

	private void Update()
	{
		timer += Time.deltaTime;
	
		if (timer > rotateInterval)
		{
			if (npcTurn)
				AutoRotate();
			timer = 0f;
		}
		if (Manager.Dialog.npcState != NpcState.Talking && !npcMoving && !npcTurn)
		{
			moveCoroutine = StartCoroutine(MoveOneStep());
		}
	}

	//	사전 입력된 좌료 리스트로 이동
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
	public void MoveTowardsPosition(Vector2 targetPos)
	{
		// 애니메이션 방향 설정
		Vector2 direction = targetPos - (Vector2)transform.position.normalized;
		anim.SetFloat("y", direction.y);
		anim.SetFloat("x", direction.x);

		transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
		
		// 이동 중 여부 판단
		bool hasReached = Vector2.Distance(transform.position, targetPos) < 0.01f;

		anim.SetBool("npcMoving", !hasReached);

	}
	//	NPC와 상호작용하는 방향 체크
	public void AnimChange(Vector2 position)
	{

		if (position.y == npcPos.y)
		{   //	좌우
			if (npcPos.x - position.x == -2)
				dir = Vector2.right;
			else
				dir = Vector2.left;
			anim.SetFloat("x", dir.x);
			anim.SetFloat("y", 0);

		}
		else
		{   // 상하
			if (npcPos.y - position.y == -2)
				dir = Vector2.up;
			else
				dir = Vector2.down;
			anim.SetFloat("x", 0);
			anim.SetFloat("y", dir.y);

		}
	}
	//	코루틴 정지
	public void StopMoving()
	{
		Vector2 direction = currentDirection;
		if (moveCoroutine != null)
		{
			StopCoroutine(moveCoroutine);
			moveCoroutine = null;
		}
		npcMoving = false;
		anim.SetBool("npcMoving", false);
		anim.SetFloat("y", dir.y);
		anim.SetFloat("x", dir.x);

		Manager.Dialog.npcState = NpcState.Idle;  // 멈출 때 Idle 상태로 변경
	}

	//이동 가능 여부
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
