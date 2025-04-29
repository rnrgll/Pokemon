using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_PlayerStatusPanel : MonoBehaviour
{
	[SerializeField] private TMP_Text name_txt;
	[SerializeField] private TMP_Text id_txt;
	[SerializeField] private TMP_Text money_txt;
	[SerializeField] private TMP_Text playTime_txt;

	private void Awake()
	{
		name_txt = transform.GetChild(0).GetComponent<TMP_Text>();
		id_txt = transform.GetChild(1).GetComponent<TMP_Text>();
		money_txt = transform.GetChild(2).GetComponent<TMP_Text>();
		playTime_txt = transform.GetChild(3).GetComponent<TMP_Text>();
	}

	private void OnEnable()
	{
		RefreshUI();
	}

	private void RefreshUI()
	{
		LDH_PlayerData playerData = Manager.Data.LdhPlayerData;
		name_txt.text = playerData.PlayerName;
		id_txt.text = playerData.PlayerID;
		money_txt.text = $"{playerData.Money}원";
		playTime_txt.text = "nn : nn"; //시간 변환하는 거 쓰기
	}
	
}
