using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JHTTestHealth : MonoBehaviour
{
	JHTTestPokeClass pokeClass;
	JHTTestExp pokeExp;

	private void Awake()
	{
		pokeExp = GetComponent<JHTTestExp>();
	}

	public void TakeDamage(int amount)
	{
		pokeClass.hp -= amount;

		if (pokeClass.hp <= 0)
		{
			Die();
		}
	}
	
	public void Die()
	{
		if (!pokeClass.isMyPoke)
		{
			pokeExp.GetXP(10);
		}

		if (gameObject != null)
		{
			Destroy(gameObject);
		}
	}
}
