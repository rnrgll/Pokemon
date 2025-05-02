using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PokeManager
{
	[SerializeField] PokeClasses pokeClass;
	//[SerializeField] PokeClassType type;

	public int level;
	public int curExp;
	public int curHp;

	public PokeManager(PokeClasses pokeClass)
	{
		this.pokeClass = pokeClass;
		this.level = 1;
		this.curHp = pokeClass.maxHp;
		this.curExp = 0;
	}

	public void GetExp(int amount)
	{
		curExp += amount;
		if (curExp >= pokeClass.maxExp)
		{
			Debug.Log($"현재레벨 : {level}");
			Debug.Log($"현재체력 : {curHp}");
			level++;
			curExp -= pokeClass.maxExp;
			pokeClass.maxExp += pokeClass.maxExp / 2;
			curHp += 20;
			pokeClass.maxHp = GetMaxHp();
			Debug.Log($"증가된 체력 : {curHp}");
		}
	}

	public int GetMaxHp()
	{
		Debug.Log("최대체력 획득");
		return pokeClass.maxHp + pokeClass.maxHp / 2;
	}

	public void TakeDamage(int amount)
	{
		curHp = Mathf.Max(0, curHp - amount);
		if (curHp == 0)
		{
			Debug.Log($"{pokeClass.pokeName}이 죽음");
		}
	}

	public void Heal(int amount)
	{
		Debug.Log($"현재 체력: {curHp}");
		curHp = Mathf.Min(GetMaxHp(), curHp + amount);
		Debug.Log($"증가된 체력: {curHp}");
	}

}
