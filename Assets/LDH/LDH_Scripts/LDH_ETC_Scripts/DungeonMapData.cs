using System;
using System.Collections.Generic;
using UnityEngine;


public class DungeonMapData
{
	[Serializable]
	public struct DungeonLink
	{
		//출입구가 여러개 일 수 있으나, 일단 1개로 가정하고 개발
		public string dungeonSceneName;      // 던전 씬
		public string entranceSceneName;     // 입장 씬
		public Vector2 dungeonExitPosition;  // 던전 안 출입구 위치
		public Vector2 entrancePosition;     // 입장 씬의 입구 위치
		
	}

	private List<DungeonLink> links;
	
	public bool TryGetLink(string dungeonScene, out DungeonLink link)
	{
		foreach (DungeonLink l in links)
		{
			if (l.dungeonSceneName == dungeonScene)
			{
				link = l;
				return true;
			}
		}

		link = default;
		return false;
	}

	#region Data

	public void Init()
	{
		links = new List<DungeonLink>();
		links.Add(new DungeonLink
		{
			dungeonSceneName = "SproutTower1F", //추후 수정 예정
			entranceSceneName = Define.sceneDic["도라지시티"],
			dungeonExitPosition = new Vector2(-286f, 172f),
			entrancePosition = new Vector2(-285.959991f,172.694f) // -286 172
		});
		links.Add(new DungeonLink
		{
			dungeonSceneName = "SproutTower2F", //추후 수정 예정
			entranceSceneName = Define.sceneDic["도라지시티"],
			dungeonExitPosition = new Vector2(-286f, 172f),
			entrancePosition = new Vector2(-285.959991f, 172.694f) // -286 172
		});
		links.Add(new DungeonLink
		{
			dungeonSceneName = "SproutTower3F", //추후 수정 예정
			entranceSceneName = Define.sceneDic["도라지시티"],
			dungeonExitPosition = new Vector2(-286f, 172f),
			entrancePosition = new Vector2(-285.959991f, 172.694f) // -286 172
		});
	}
	

	#endregion
	
}


