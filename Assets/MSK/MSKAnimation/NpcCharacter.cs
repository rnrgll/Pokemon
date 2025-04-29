using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class NpcCharacter : MonoBehaviour
{
	NPCAnimations npcAnimator;
	[SerializeField] int moveValue = 2;
	private void Awake()
	{
		npcAnimator = GetComponent<NPCAnimations>();
	}
	public IEnumerator NpcMove(Vector2 direction)
	{
		npcAnimator.MoveX = Mathf.Clamp(direction.x, -1f,1f);
		npcAnimator.MoveY = Mathf.Clamp(direction.y, -1f, 1f);

		var targetPos = transform.position;
		targetPos.x += direction.x;
		targetPos.y += direction.y;
		if (IsWalkAble(targetPos))
			yield break;

		npcAnimator.IsNpcMoving = true;
		
		while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
		{
			transform.position = Vector2.MoveTowards(transform.position, targetPos, moveValue * Time.deltaTime);
			yield return null;
		}
		transform.position = targetPos;

		npcAnimator.IsNpcMoving = false;
	}

	private bool IsWalkAble(Vector2 targetPos)
	{
		bool isWalk = true;
		Vector2 inputDir = (targetPos - (Vector2)transform.position).normalized;
		RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + inputDir * 0.1f, inputDir, 1f);
		if (hit && (hit.transform.CompareTag("Wall") || hit.transform.CompareTag("NPC")|| hit.transform.CompareTag("Player")))
		{
			return isWalk = false;
		}
		return isWalk;
	}
}
