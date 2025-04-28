using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PokeEvent : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
			OnPokeEvent(collision);
	}

	public abstract void OnPokeEvent(Collider2D player);
}
