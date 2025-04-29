using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeHealth : MonoBehaviour
{
    PokeController controller;
	PokeExperience experience;

	public event Action OnDie;
	private void Awake()
	{
		controller = GetComponent<PokeController>();
		experience = GetComponent<PokeExperience>();
	}
	private void OnEnable()
	{
		OnDie += Die;
	}
	private void OnDisable()
	{
		OnDie -= Die;
	}


	public void TakeDamage(int damage)
	{
		if (controller.gameObject.layer == 11)
		{
			controller.curHp -= damage;
		}

		if (controller.curHp <= 0)
		{
			OnDie.Invoke();
		}
	}

	private void Die()
	{
		Destroy(controller.gameObject);
	}
}
