using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class LDH_PlayerData
{
	[SerializeField] private string playerName;
	public string PlayerName => playerName;

	[SerializeField] private string playerID;
	public string PlayerID => playerID;
	

	[SerializeField] private int money;
	public int Money => money;
	
	[SerializeField] private float playTime;
	public float PlayTime => playTime;

	[SerializeField] private bool[] hasBadges = new bool[8];
	public bool[] HasBadges => hasBadges;


	// 초기화 함수 (처음 게임 시작할 때 호출)
	public void Init()
	{
		playerName = "골드";
		playerID = Random.Range(10000, 100000).ToString();
		money = 3000;
		
		playTime = 0f;
        
		for (int i = 0; i < hasBadges.Length; i++)
		{
			hasBadges[i] = false;
		}
	}
	
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

	// 플레이 타임 갱신
	public void UpdatePlayTime(float deltaTime)
	{
		playTime += deltaTime;
	}

	// 배지 획득
	public void GetBadge(int index)
	{
		if (index >= 0 && index < hasBadges.Length)
		{
			hasBadges[index] = true;
		}
	}
	

}
