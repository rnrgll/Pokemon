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
			}
		};
	}


	public List<WildEncounterData> GetDataByScene(string sceneName)
	{
		// TODO : bool 날씨 추가하던 말던해야함
		var data = dataBySceneName[sceneName][true];
		if (data != null)
			return dataBySceneName[sceneName][true];
		else
			return null;
	}
}
