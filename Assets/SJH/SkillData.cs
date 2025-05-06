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

	// pp 일정량 감소
	public int DecreasePP(int value)
	{
		CurPP = Mathf.Max(0, CurPP - value);
		return CurPP;
	}

	// pp 1 감소
	public int DecreasePP()
	{
		CurPP = Mathf.Max(0, CurPP - 1);
		return CurPP;
	}

	// pp 1 회복
	public int IncreasePP(int value)
	{
		CurPP = Mathf.Min(MaxPP, CurPP + value);
		return CurPP;
	}

	// 최대 PP로 변경 / 센터용
	public int IncreaseMaxPP()
	{
		CurPP = MaxPP;
		return CurPP;
	}
}
