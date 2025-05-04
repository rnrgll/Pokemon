using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
	[SerializeField] Transform player;
	[SerializeField] Player p;
	[SerializeField] public Queue<Vector3> prevPos = new Queue<Vector3>();
	[SerializeField] int followDelay = 5;

	Coroutine watchCoroutine;
	Coroutine followCoroutine;

	void Start()
	{
		StartCoroutine(InitFollower());
	}

	IEnumerator InitFollower()
	{
		// 플레이어가 로딩되기까지 기다림
		yield return new WaitUntil(() => Manager.Game.Player != null);

		player = Manager.Game.Player.transform;
		p = player.gameObject.GetComponent<Player>();

		followCoroutine = StartCoroutine(FollowPlayer());
	}

	IEnumerator FollowPlayer()
	{
		while (true)
		{
			if (p.prevPosQueue.Count >= 1)
			{
				Vector2 startPos = transform.position;
				Vector3 targetPos = p.prevPosQueue.Dequeue(); // 저장된 위치를 꺼내서

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
