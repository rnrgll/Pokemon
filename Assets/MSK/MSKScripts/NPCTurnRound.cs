using System.Collections;
using UnityEngine;

public class NPCTrainer : MonoBehaviour
{
	private int directionIndex = 0;
	Animator anim;

	public float rotateInterval = 2.0f; // 회전 간격 (초)
	private Coroutine rotateCoroutine;

	// 빙글빙글
	private readonly Vector2[] directions = new Vector2[]
	{
		Vector2.right,
		Vector2.down,
		Vector2.left,
		Vector2.up
	};
	private void Awake()
	{
		anim = GetComponent<Animator>();
	}


	private void Start()
	{
		rotateCoroutine = StartCoroutine(AutoRotate());
	}

	private IEnumerator AutoRotate()
	{
		while (true)
		{
			Vector2 dir = directions[directionIndex];
			anim.SetFloat("x", dir.x);
			anim.SetFloat("y", dir.y);

			directionIndex = (directionIndex + 1) % directions.Length;

			yield return new WaitForSeconds(rotateInterval);
		}
	}

}
