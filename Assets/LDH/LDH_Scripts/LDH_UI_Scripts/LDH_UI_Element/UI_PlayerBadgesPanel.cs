using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_PlayerBadgesPanel : MonoBehaviour
{
	[SerializeField] private TMP_Text name_txt;
	[SerializeField] private TMP_Text id_txt;
	[SerializeField] private TMP_Text money_txt;
	[SerializeField] private Transform badges;

	private void Awake()
	{
		name_txt = transform.GetChild(0).GetComponent<TMP_Text>();
		id_txt = transform.GetChild(1).GetComponent<TMP_Text>();
		money_txt = transform.GetChild(2).GetComponent<TMP_Text>();
		badges = transform.GetChild(3);
	}

	private void OnEnable()
	{
		RefreshUI();
	}

	private void RefreshUI()
	{
		PlayerData playerData = Manager.Data.PlayerData;
		name_txt.text = playerData.PlayerName;
		id_txt.text = playerData.PlayerID;
		money_txt.text = $"{playerData.Money}원";
		UpdateBadge();
	}

	private void UpdateBadge()
	{
		bool[] hasBadges = Manager.Data.PlayerData.HasBadges;
		for (int i = 0; i < hasBadges.Length; i++)
		{
			badges.GetChild(i).GetChild(1).gameObject.SetActive(hasBadges[i]);
		}
	}
    
}
