using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class NpcMover : MonoBehaviour
{
	// 도착할 위치 : 기본은 시작 위치
	[SerializeField] public Vector2 exitPos;

	// 이동할 위치 2 단위로
	[SerializeField] List<Vector2> destinationPoints;

	// 이동 시간
	[SerializeField] float moveDuration = 0.3f;

	// 회전 시간
	public float rotateInterval = 2.0f;

	// 이동 시간
	[SerializeField] float moveSpeed;

	// 엔피시 이동 여부
	[SerializeField] bool npcMoving;

	// 엔피시 회전 여부
	public bool isNPCTurn;

	private int directionIndex = 0;

	int moveIndex = 0;

	float timer;

	bool npcTurn;

	[SerializeField] Vector2 currentDirection;
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

	Coroutine npcMoveCoroutione;
	Coroutine npcTurnCoroutione;
	[SerializeField] public bool isNPCMoveCheck;
	[SerializeField] public bool isNPCTurnCheck;

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
		//anim = npcController.anim;
		anim = GetComponent<Animator>();
		currentDirection = Vector2.down;
		dir = currentDirection;
		anim.SetFloat("x", currentDirection.x);
		anim.SetFloat("y", currentDirection.y);
	}

	private void Update()
	{
		timer += Time.deltaTime;

		if (isNPCMoveCheck && npcMoveCoroutione == null)
		{
			npcMoveCoroutione = StartCoroutine(NPCMove());
		}
		else if (isNPCTurnCheck && npcTurnCoroutione == null)
		{
			npcTurnCoroutione = StartCoroutine(NPCTurn());
		}

		//if (timer > rotateInterval)
		//{
		//	if (isNPCTurn)
		//		AutoRotate();
		//	timer = 0f;
		//}
		//if (Manager.Dialog.npcState != NpcState.Talking && !npcMoving && !npcTurn)
		//{
		//	moveCoroutine = StartCoroutine(MoveOneStep());
		//}
	}

	//	사전 입력된 좌료 리스트로 이동
	public IEnumerator MoveOneStep()
	{
		npcMoving = true;

		Vector2 currentPos = (Vector2)transform.position;
		Vector2 targetPos = destinationPoints[moveIndex];
		currentDirection = (targetPos - currentPos).normalized;

		if (anim != null)
		{
			anim.SetFloat("x", currentDirection.x);
			anim.SetFloat("y", currentDirection.y);
			anim.SetBool("npcMoving", true);
		}

		while (Vector2.Distance(transform.position, targetPos) > 0.1f)
		{
			// 이동 중에도 전방 장애물 감지
			if (!IsWalkAble(currentDirection))
			{
				// 애니메이션 멈추고 대기
				if (anim != null)
					anim.SetBool("npcMoving", false);
				yield return null;
				continue;
			}
			else
			{
				// 다시 이동 시작
				if (anim != null)
					anim.SetBool("npcMoving", true);
				transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
			}

			yield return null;
		}

		transform.position = targetPos;
		if (anim != null)
			anim.SetBool("npcMoving", false);
		if (destinationPoints.Count > 0)
			moveIndex = (moveIndex + 1) % destinationPoints.Count;

		yield return new WaitForSeconds(1f);
		npcMoving = false;

	}

	IEnumerator NPCMove()
	{
		Debug.Log("엔피시 이동");

		if ((Vector2)transform.position == destinationPoints[destinationPoints.Count - 1])
		{
			Debug.Log("엔피시 도착 이동 종료");
			isNPCMoveCheck = false;
			anim.SetBool("npcMoving", isNPCMoveCheck);
			yield break;
		}
		if (destinationPoints.Count >= 1)
		{
			Debug.Log("엔피시 이동 시작");
			Vector3 startPos = transform.position;
			Vector3 targetPos = destinationPoints[moveIndex];
			dir = (targetPos - startPos).normalized;

			if (dir != Vector2.zero)
				currentDirection = dir;

			// 플레이어를 체크하려면 레이를 좀 낮게
			Vector2 rayStartPos = (Vector2)startPos + Vector2.down * 0.3f;

			// 이동 전에 Raycast 검사
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

			// 이동 시작
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

			// 다음 목적지로
			if (moveIndex < destinationPoints.Count - 1)
				moveIndex++;
			else if (moveIndex >= destinationPoints.Count - 1)
			{
				isNPCMoveCheck = false;
				anim.SetBool("npcMoving", isNPCMoveCheck);
			}

			// 도착 후 코루틴 null
			npcMoveCoroutione = null;
		}
		else
		{
			yield return null;
		}
	}

	IEnumerator NPCTurn()
	{
		Debug.Log("엔피시 회전");

		int ran = UnityEngine.Random.Range(0, 4);
		Vector2 dir = directions[ran];

		currentDirection = dir;

		anim.SetFloat("x", currentDirection.x);
		anim.SetFloat("y", currentDirection.y);

		yield return new WaitForSeconds(2f);	

		npcTurnCoroutione = null;
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
