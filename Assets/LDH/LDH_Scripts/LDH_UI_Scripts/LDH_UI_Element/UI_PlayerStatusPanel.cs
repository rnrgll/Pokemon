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

	//매 초마다 시간 ui 업데이트하는 코루틴
	private Coroutine timeUpdateCoroutine;
	private WaitForSeconds delay;
	
	private void Awake()
	{
		name_txt = transform.GetChild(0).GetComponent<TMP_Text>();
		id_txt = transform.GetChild(1).GetComponent<TMP_Text>();
		money_txt = transform.GetChild(2).GetComponent<TMP_Text>();
		playTime_txt = transform.GetChild(3).GetComponent<TMP_Text>();

		delay= new WaitForSeconds(0.5f);
	}

	private void OnEnable()
	{
		RefreshUI();
		
		//코루틴 시작
		if (timeUpdateCoroutine == null)
			timeUpdateCoroutine = StartCoroutine(UpdatePlayTimeCoroutine());
	}

	private void OnDisable()
	{
		if (timeUpdateCoroutine != null)
		{
			StopCoroutine(timeUpdateCoroutine);
			timeUpdateCoroutine = null;
		}
	}

	private void RefreshUI()
	{
		PlayerData playerData = Manager.Data.PlayerData;
		name_txt.text = playerData.PlayerName;
		id_txt.text = playerData.PlayerID;
		money_txt.text = $"{playerData.Money}원";
	}

	// 플레이 시간 텍스트를 0.5초마다 갱신하며 콜론(:)을 깜빡이게 하는 코루틴
	private IEnumerator UpdatePlayTimeCoroutine()
	{
		bool showColon = true;
		while (true)
		{
			float elapsedTime = Manager.Data.PlayerData.GetPlayTime();
			playTime_txt.text = Util.FormatTimeHMWithBlink(elapsedTime, showColon);
			showColon = !showColon;
			yield return delay;
		}
	}
}
