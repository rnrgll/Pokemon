using UnityEngine;

public class NPCTrainer : MonoBehaviour
{
	[SerializeField] Dialog dialog;

	public float detectionRange = 8f;
	public bool isChasingPlayer { get; private set; } = false;

	bool isBattled;
	private NpcMover npcMover;
	Vector2 currentDirection;
	Vector2 playerPos;
	Animator anim;

	Coroutine detectCoroutine;


	private void Awake()
	{
		isBattled = false;  //	기본 전투 미진행
		npcMover = GetComponent<NpcMover>();
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		// 전투하지 않았을 경우
		if (!isBattled)
		{
			currentDirection = npcMover.currentDirection;
			isChasingPlayer = PcDetect(currentDirection, out playerPos);
			Debug.Log($"추격상태 : {isChasingPlayer}");
			if (isChasingPlayer)
			{
				Manager.Game.Player.State = Define.PlayerState.Dialog;
				Debug.Log($"추격 호출");
				npcMover.MoveTowardsPosition(playerPos - currentDirection * 2);
				//	도착 확인 후
				// 다이얼로그 매니저 호출
				if (isMoved(currentDirection))
				{
					Manager.Dialog.StartDialogue(dialog);
					//	배틀매니저 호출
					isBattled = true;
					Manager.Game.Player.State = Define.PlayerState.Field;
				}
			}
		}

	}
	private bool isMoved(Vector2 currentDirection)
	{

		Vector2 startPos;
		startPos.x = transform.position.x;
		startPos.y = transform.position.y - 0.1f;   // PC box에 위치 조정

		RaycastHit2D hit = Physics2D.Raycast(startPos + currentDirection * 1.1f, currentDirection, 0.3f);

		// 자기 자신 무시
		if (hit.collider != null && hit.collider.transform.root == transform.root)
		{
			return false;
		}

		if (hit.collider != null)
		{
			Debug.Log($"{hit.transform.name}에 명중");
			string hitTag = hit.transform.tag;
			if (hitTag == "Player")
			{
				playerPos.x = hit.transform.position.x;
				playerPos.y = hit.transform.position.y;
				return true;
			}
		}
		else
		{
			Debug.Log("명중 없음");
		}

		return false;
	}
	private bool PcDetect(Vector2 currentDirection, out Vector2 playerPos)
	{

		Vector2 startPos;
		startPos.x = transform.position.x;
		startPos.y = transform.position.y - 0.1f;   // PC box에 위치 조정

		playerPos.x = 0;
		playerPos.y = 0;
		RaycastHit2D hit = Physics2D.Raycast(startPos + currentDirection * 1.1f, currentDirection, detectionRange);

		// 자기 자신 무시
		if (hit.collider != null && hit.collider.transform.root == transform.root)
		{
			return false;
		}

		if (hit.collider != null)
		{
			Debug.Log($"{hit.transform.name}에 명중");
			string hitTag = hit.transform.tag;
			if (hitTag == "Player")
			{
				playerPos.x = hit.transform.position.x;
				playerPos.y = hit.transform.position.y;

				Debug.Log($"{playerPos.x}, {playerPos.y}로 이동");
				return true;
			}
		}
		else
		{
			Debug.Log("명중 없음");
		}

		return false;
	}
}