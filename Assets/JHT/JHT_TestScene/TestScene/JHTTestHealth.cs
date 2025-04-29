using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


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
		if (pokeClass == null) pokeClass = GetComponent<JHTTestPokeClass>();

		pokeClass.hp -= amount;
		Debug.Log($"데미지 {amount}만큼 {gameObject.name}이 받았습니다 현재 체력 {pokeClass.hp}");
		if (pokeClass.hp <= 0)
		{
			Die();
		}
	}
	
	public void Die()
	{
		//if (!pokeClass.isMyPoke)
		//{
		//	pokeExp.GetXP(10);
		//}

		if (gameObject != null)
		{
			Destroy(gameObject);
		}
	}

	public void DebugMessage()
	{
		Debug.Log(pokeClass.hp);
	}
}
