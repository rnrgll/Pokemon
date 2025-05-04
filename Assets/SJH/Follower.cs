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

		watchCoroutine = StartCoroutine(WatchPlayer());
		followCoroutine = StartCoroutine(FollowPlayer());
	}

	IEnumerator WatchPlayer()
	{
		Vector3 lastRecorded = player.position;

		while (true)
		{
			// 이동이 끝났고 (2칸 이상 이동), 새로운 위치라면
			if (Vector3.Distance(player.position, lastRecorded) >= 2f)
			{
				lastRecorded = player.position;

				// 현재 방향의 반대쪽으로 2 보정해서 기록
				Vector3 offset = -(Vector3)p.currentDirection * 2f;
				Vector3 followerTarget = lastRecorded + offset;

				prevPos.Enqueue(followerTarget);

				// 큐 제한
				if (prevPos.Count > followDelay)
					prevPos.Dequeue();
			}

			yield return null;
		}
	}

	IEnumerator FollowPlayer()
	{
		while (true)
		{
			if (prevPos.Count >= 1)
			{
				Vector2 startPos = transform.position;
				Vector3 targetPos = prevPos.Dequeue(); // 저장된 위치를 꺼내서

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
