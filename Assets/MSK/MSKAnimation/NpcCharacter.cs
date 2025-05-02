using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class NpcCharacter : MonoBehaviour
{
	Animator npcAnimator;
	[SerializeField] int moveValue = 2;
	

	private void Awake()
	{
		npcAnimator = GetComponent<Animator>();
	}
	public IEnumerator NpcMove(Vector2 direction)
	{
		//npcAnimator.MoveX = Mathf.Clamp(direction.x, -1f,1f);
		//npcAnimator.MoveY = Mathf.Clamp(direction.y, -1f, 1f);

		var targetPos = transform.position;
		targetPos.x += direction.x;
		targetPos.y += direction.y;
		//if (IsWalkAble(targetPos))
		//	yield break;

		//NpcMover.npcMoving = true;
		
		while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
		{
			transform.position = Vector2.MoveTowards(transform.position, targetPos, moveValue * Time.deltaTime);
			yield return null;
		}
		transform.position = targetPos;

		//npcAnimator.npcMoving = false;
	}
}
