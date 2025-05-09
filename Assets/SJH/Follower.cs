using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
	[SerializeField] Transform player;
	[SerializeField] Player p;
	[SerializeField] public Queue<Vector3> prevPos = new Queue<Vector3>();

	Coroutine initCoroutine;
	Coroutine followCoroutine;

	[SerializeField] Vector2 curDirection;
	[SerializeField] Animator anim;

	void Start()
	{
		DontDestroyOnLoad(gameObject);
		initCoroutine = StartCoroutine(InitFollower());
	}
	
	IEnumerator InitFollower()
	{
		// 플레이어가 로딩되기까지 기다림
		yield return new WaitUntil(() => Manager.Game.Player != null);

		player = Manager.Game.Player.transform;
		p = player.gameObject.GetComponent<Player>();


		anim = GetComponent<Animator>();

		curDirection = p.currentDirection;
		anim.SetFloat("x", curDirection.x);
		anim.SetFloat("y", curDirection.y);

		followCoroutine = StartCoroutine(FollowPlayer());
	}

	IEnumerator FollowPlayer()
	{
		while (true)
		{
			if (p.prevPosQueue.Count >= 1)
			{
				Vector3 startPos = transform.position;
				Vector3 targetPos = p.prevPosQueue.Dequeue(); // 저장된 위치를 꺼내서

				// 포켓몬 방향
				curDirection = (targetPos - startPos).normalized;
				// 애니메이션 전환
				anim.SetFloat("x", curDirection.x);
				anim.SetFloat("y", curDirection.y);

				float time = 0;
				while (time < p.moveDuration)
				{
					time += Time.deltaTime;
					float percent = time / p.moveDuration;
					transform.position = Vector2.Lerp(startPos, targetPos, percent);
					yield return null;
				}
				transform.position = targetPos;
			}
			else
			{
				yield return null;
			}
		}
	}
}
