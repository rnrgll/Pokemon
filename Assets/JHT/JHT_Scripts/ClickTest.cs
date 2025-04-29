using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTest : MonoBehaviour
{
	PokeHealth pokeHealth;

	private void Start()
	{
		pokeHealth = FindObjectOfType<PokeHealth>();
	}

	public void Click()
	{
		if (pokeHealth == null) pokeHealth = FindObjectOfType<PokeHealth>();
		pokeHealth.TakeDamage(5);
	}
}
