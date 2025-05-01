using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillSData
{
	//Init1
	Dictionary<int, SkillS> skillSData;

	//Init2
	List<SkillS> skillList;
	Dictionary<string, List<SkillS>> skillDatas;

	public string name;
	public string description;
	public int damage;
	public SkillType skillType;

	public void Init1()
	{
		skillSData = new Dictionary<int, SkillS>()
		{
			[8] = new SkillS
			(
				name = "잎날가르기",
				description = "적에게 10의 데미지를 입힙니다",
				damage = 10,
				skillType = SkillType.Physical
			)
		};
	}

	public void Init2()
	{
		skillDatas = new Dictionary<string, List<SkillS>>()
		{
			["치코리타"] = new List<SkillS>()
			{
				new SkillS
				(
					name = "잎날가르기",
					description = "적에게 10의 데미지를 입힙니다",
					damage = 10,
					skillType = SkillType.Physical
				),
				new SkillS
				(
					name = "리플렉터",
					description = "적의 방어력을 10 감소시킵니다",
					damage = 10,
					skillType = SkillType.Status
				)
			}
		};
	}
}
