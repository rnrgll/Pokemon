using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_PlayerBadgesPanel : MonoBehaviour
{
	[SerializeField] private TMP_Text name_txt;
	[SerializeField] private TMP_Text id_txt;
	[SerializeField] private TMP_Text money_txt;
	

	private void Awake()
	{
		name_txt = transform.GetChild(0).GetComponent<TMP_Text>();
		id_txt = transform.GetChild(1).GetComponent<TMP_Text>();
		money_txt = transform.GetChild(2).GetComponent<TMP_Text>();
		
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
		
	}
    
}
