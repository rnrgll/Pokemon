using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JHTTestExp : MonoBehaviour
{
	public event Action OnLevelUp;
	JHTTestPokeClass pokeClass;

	private void Awake()
	{
		pokeClass = ScriptableObject.CreateInstance<JHTTestPokeClass>();
		if (pokeClass == null)
		{
			Debug.LogError("pokeClass가 할당안됨 JHTTestExp에서 pokeClass를 연결해야함");
		}
		
	}

	private void OnEnable()
	{
		OnLevelUp += LevelUp;
	}

	private void OnDisable()
	{
		OnLevelUp -= LevelUp;
	}

	public void GetXP(int amount)
	{
		pokeClass.exp += amount;
		if (pokeClass.exp >= 10)
		{
			OnLevelUp.Invoke();
		}
	}

	public void LevelUp()
	{
		if (pokeClass == null)
		{
			Debug.LogError("LevelUp 호출 시 pokeClass가 null입니다.");
			return;
		}
		pokeClass.level++;
		pokeClass.damage += 10;
		pokeClass.hp += 20;
		pokeClass.prefabIndex++;
		UpGrade(pokeClass);
	}

	public void UpGrade(JHTTestPokeClass pokeClass)
	{
		if (pokeClass.prefabIndex < pokeClass.levelPrefab.Length)
		{
			if (pokeClass.curPrefab != null)
			{
				Destroy(pokeClass.curPrefab);
			}
			pokeClass.curPrefab = Instantiate(pokeClass.levelPrefab[pokeClass.prefabIndex]);
		}
	}
}
