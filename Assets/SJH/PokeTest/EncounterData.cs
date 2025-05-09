using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterData
{
	private Dictionary<string, Dictionary<bool, List<WildEncounterData>>> dataBySceneName;

	public void Init()
	{
		dataBySceneName = new Dictionary<string, Dictionary<bool, List<WildEncounterData>>>()
		{
			["Route29"] = new Dictionary<bool, List<WildEncounterData>>()
			{
				[true] = new List<WildEncounterData>() // 낮
				{
					new WildEncounterData("구구",   2, 4, 55),
					new WildEncounterData("꼬렛",   4, 4,  5),
					new WildEncounterData("꼬리선", 2, 3, 40),
				},
				[false] = new List<WildEncounterData>() // 밤
				{
					new WildEncounterData("꼬렛",   2, 3, 15),
					new WildEncounterData("부우부", 2, 4, 85),
				}
			},
			["Route46"] = new Dictionary<bool, List<WildEncounterData>>()
			{
				[true] = new List<WildEncounterData>() // 낮
				{
					new WildEncounterData("꼬렛",   2, 4, 25),
					new WildEncounterData("깨비참", 2, 3, 35),
					new WildEncounterData("꼬마돌", 2, 3, 40),
				},
				[false] = new List<WildEncounterData>() // 밤
				{
					new WildEncounterData("꼬렛",   2, 3, 55),
					new WildEncounterData("꼬마돌", 2, 4, 45),
				}
			},
			["Route30"] = new Dictionary<bool, List<WildEncounterData>>()
			{
				[true] = new List<WildEncounterData>() // 낮
				{
					new WildEncounterData("캐터피", 3, 4, 35),
					new WildEncounterData("단데기", 4, 5, 15),
					new WildEncounterData("뿔충이", 3, 4, 35),
					new WildEncounterData("딱충이", 4, 5, 15),
					new WildEncounterData("구구",   2, 4, 50),
				},
				[false] = new List<WildEncounterData>() // 밤
				{
					new WildEncounterData("꼬렛",   3, 4, 40),
					new WildEncounterData("부우부", 4, 4, 30),
					new WildEncounterData("페이검", 3, 3, 30),
				}
			},
			["Route31"] = new Dictionary<bool, List<WildEncounterData>>()
			{
				[true] = new List<WildEncounterData>() // 낮
				{
					new WildEncounterData("캐터피", 4, 5, 35),
					new WildEncounterData("단데기", 5, 6, 15),
					new WildEncounterData("뿔충이", 4, 5, 35),
					new WildEncounterData("딱충이", 5, 6, 15),
					new WildEncounterData("구구",   3, 3, 30),
					new WildEncounterData("모다피", 3, 3, 20),
				},
				[false] = new List<WildEncounterData>() // 밤
				{
					new WildEncounterData("꼬렛",   4, 5, 40),
					new WildEncounterData("부우부", 5, 5, 10),
					new WildEncounterData("페이검", 4, 4, 30),
					new WildEncounterData("모다피", 3, 3, 20),
				}
			},
			["SproutTower"] = new Dictionary<bool, List<WildEncounterData>>()
			{
				[true] = new List<WildEncounterData>() // 낮
				{
					new WildEncounterData("꼬렛",   3, 6, 100),
				},
				[false] = new List<WildEncounterData>() // 밤
				{
					new WildEncounterData("꼬렛",   3, 6, 15),
					new WildEncounterData("고오스", 3, 6, 85),
				}
			},
		};
	}


	public List<WildEncounterData> GetDataByScene(string sceneName)
	{
		// 그냥 낮 + 밤 테이블 반환
		var result = new List<WildEncounterData>();
		result.AddRange(dataBySceneName[sceneName][true]);
		result.AddRange(dataBySceneName[sceneName][false]);
		return result;
	}
}
