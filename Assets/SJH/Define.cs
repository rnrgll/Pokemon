using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
	public static Dictionary<string, string> sceneDic = new Dictionary<string, string>()
	{
		["주인공집2층"] = "PlayerHouse2F",
		["주인공집1층"] = "PlayerHouse1F",
		["연두마을"] = "NewBarkTown",
		["연구소"] = "ProfessorLab",
		["29번도로"] = "Route29",

	};

	public enum PortalType
	{
		Stair,      // 트리거 방식으로 씬변경
		Foothold    // Stay 일때 특정 방향키 입력하면 씬변경
	};

	public enum PlayerState
	{
		Field,			// 필드
		SceneChange,    // 씬 이동중
		Battle,			// 배틀중
		UI,				// UI 활성화중
		Menu,			// Menu 활성화중
	};
}
