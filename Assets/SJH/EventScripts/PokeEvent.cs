using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PokeEvent : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			OnPokeEvent(collision.gameObject);
			Debug.Log("이벤트 트리거 실행");
		}
	}

	public abstract void OnPokeEvent(GameObject player);
}
