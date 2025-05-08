using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndEvent : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.CompareTag("Player"))
		{
			// 체육관 클리어
			Debug.Log("관장 트리거 실행");
			if (Manager.Event.gymEvent)
			{
				// TODO : 씬전환
			}
		}
	}
}
