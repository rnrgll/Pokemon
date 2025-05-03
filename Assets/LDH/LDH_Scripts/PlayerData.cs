using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class PlayerData
{
	//========플레이어 기본 정보==========//
	[SerializeField] private string playerName;
	[SerializeField] private string playerID; //랜덤생성
	[SerializeField] private int money;
	public string PlayerName => playerName;
	public string PlayerID => playerID;
	public int Money => money;
	
	//========트레이너 카드 관련 정보==========//
	[SerializeField] private float playStartTime;
	[SerializeField] private bool[] hasBadges = new bool[8];
	public bool[] HasBadges => hasBadges;


	//==========인벤토리 정보===========//
	private Inventory _inventory;
	public Inventory Inventory => _inventory;
	public IReadOnlyList<InventorySlot> PlayerInventory => _inventory.Slots; //앝은 API 제공용..이 필요한가? 모르겠다
	


	#region Initailization
	
	// 초기화 함수 (처음 게임 시작할 때 호출)
	public void Init()
	{
		//플레이어 이름 입력받으면 데이터 초기화해주는걸로 바꿀 예정
		InitData("골드");

		
	}

	private void InitData(string name) //생성자로 바꿀지 고민중
	{
		//초기 데이터로 초기화
		playerName = name;
		playerID = Random.Range(10000, 100000).ToString();
		money = 3000;

		playStartTime = Time.time; //시작 시간
		
		for (int i = 0; i < hasBadges.Length; i++)
		{
			hasBadges[i] = false;
		}
		
		//인벤토리 생성
		_inventory = new();
		_inventory.Init();
	}

	#endregion

	
	#region PlayerInfo Get/Modify API
	
	// 플레이어 이름 변경
	public void SetPlayerName(string newName)
	{
		playerName = newName;
	}

	// 돈 추가
	public void AddMoney(int amount)
	{
		money += amount;
	}

	// 돈 사용
	public bool SpendMoney(int amount)
	{
		if (money >= amount)
		{
			money -= amount;
			return true;
		}
		return false;
	}

	#endregion


	#region Trainer Card Info API

	/// <summary>
	/// 플레이 누적 시간 초 단위로 반환(float)
	/// </summary>
	/// <returns>Time.time - 플레이 시작 시간</returns>
	public float GetPlayTime()
	{
		//Debug.Log(Time.time - playStartTime);
		return Time.time - playStartTime;
	}

	// 배지 획득
	public void GetBadge(int index)
	{
		if (index >= 0 && index < hasBadges.Length)
		{
			hasBadges[index] = true;
		}
	}
	
	#endregion

	

}
