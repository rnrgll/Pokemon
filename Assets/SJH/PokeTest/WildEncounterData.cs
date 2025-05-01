using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[SerializeField]
public struct WildEncounterData
{
	public string Name;
	public int MinLevel;
	public int MaxLevel;
	public int Rate;

	public WildEncounterData(string name, int minLevel, int maxLevel, int rate)
	{
		Name = name;
		MinLevel = minLevel;
		MaxLevel = maxLevel;
		Rate = rate;
	}

	public int GetRandomLevel()
	{
		 return Random.Range(MinLevel, MaxLevel + 1);
	}
}
