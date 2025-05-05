using UnityEngine;

public class NPCTrainer : MonoBehaviour
{
	public float detectionRange = 8f;
	public bool isChasingPlayer { get; private set; } = false;
	
	bool isBattled;

	private NpcMover npcMover;
	Vector2 currentDirection = Vector2.down;
	Vector2 playerPos;
	private void Awake()
	{	
		isBattled = false;	//	기본 전투 미진행
		npcMover = GetComponent<NpcMover>();
	}
	private void FixedUpdate()
	{	// 전투하지 않았을 경우
		if (!isBattled)
		{
			isChasingPlayer = PcDetect(currentDirection, out playerPos);
			Debug.Log($"추격상태 : {isChasingPlayer}");
			if (isChasingPlayer)
			{
				Debug.Log($"추격 호출");
				npcMover.MoveTowardsDirection(playerPos);
			}
		}
	}

	private bool PcDetect(Vector2 currentDirection, out Vector2 playerPos)
	{
		Vector2 startPos;
		startPos.x = transform.position.x;
		startPos.y = transform.position.y - 0.5f;

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