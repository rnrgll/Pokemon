using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SkillData
{
	public string Name;
	public int CurPP;
	public int MaxPP;

	public SkillData(string name, int curPP, int maxPP)
	{
		Name = name;
		CurPP = curPP;
		MaxPP = maxPP;
	}
}
