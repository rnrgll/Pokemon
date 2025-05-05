using System.Collections;
using UnityEngine;

public class NPCTrainer : MonoBehaviour
{
	public float detectionRange = 8f;
	public bool isChasingPlayer { get; private set; } = false;

	private GameObject player;
	private NpcMover npcMover;
	private NPCTurnRound npcTurn;
	Vector2 currentDirection = Vector2.down;


	private void FixedUpdate()
	{
		PcDetect(currentDirection);
	}

	private bool PcDetect(Vector2 currentDirection)
	{
		Vector2 startPos;
		startPos.x = transform.position.x;
		startPos.y = transform.position.y - 0.5f;
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