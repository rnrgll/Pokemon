using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
	[SerializeField] private RectTransform playerPanel;
	[SerializeField] private RectTransform enemyPanel;
	private Vector2 pPanelOrigin;
	private Vector2 ePanelOrigin;
	
	[SerializeField] private TMP_Text playerNameText;
	[SerializeField] private TMP_Text playerLevelText;
	[SerializeField] private UI_HpBarController playerHpBar;
	[SerializeField] private UI_ExpBarController playerExpBar;
	public UI_ExpBarController PlayerExpBar => playerExpBar;
	

	// 추후 상태표시창 추가 가능

	[SerializeField] private TMP_Text enemyNameText;
	[SerializeField] private TMP_Text enemyLevelText;
	[SerializeField] private UI_HpBarController enemyHpBar;
	// 추후 상태 표시창 추가가능

	[SerializeField] private TMP_Text playerCondition;
	[SerializeField] private TMP_Text enemyCondition;
	
	
	
	public void InitPlayerExp(Pokémon p)
	{
		int targetVal = p.GetExpByLevel(p.level + 1) - p.GetExpByLevel(p.level);
		int curVal = p.curExp - p.GetExpByLevel(p.level);
		playerExpBar.SetExp(curVal, targetVal);
	}
	
	

	public void SetPlayerHUD(Pokémon p, bool playAnim=true)
	{
		int prevHp = (int)playerHpBar.HpSlider.value;
		playerNameText.text = p.pokeName;            // 이름
		playerLevelText.text = $"Lv {p.level}";  // 레벨
		playerHpBar.SetHp(p.hp, p.maxHp);
		playerCondition.text = Define.GetKoreanState[p.condition];
		playerCondition.gameObject.SetActive(p.condition!=Define.StatusCondition.Normal);

		
		if(!playAnim) return;
		
		playerHpBar.AnimationHpChange(prevHp, p.hp, p.maxHp);
		
		// 추후 상태 표시창 추가가능
	}

	public void SetEnemyHUD(Pokémon e, bool playAnim=true)
	{
		int prevHp = (int)enemyHpBar.HpSlider.value;
		enemyNameText.text = e.pokeName;
		enemyLevelText.text = $"Lv {e.level}";
		enemyNameText.text = e.pokeName;
		enemyLevelText.text = $"Lv {e.level}";
		enemyHpBar.SetHp(e.hp, e.maxHp);
		
		enemyCondition.text = Define.GetKoreanState[e.condition];
		enemyCondition.gameObject.SetActive(e.condition!=Define.StatusCondition.Normal);
		
		if(!playAnim) return;
		
		enemyHpBar.AnimationHpChange(prevHp, e.hp, e.maxHp);

		//Debug.Log($"{e.pokeName} : Lv. {e.level} [{e.hp} / {e.maxHp}]");
		// 추후 상태 표시창 추가가능
	}


	private void Start()
	{
		pPanelOrigin = playerPanel.anchoredPosition;
		ePanelOrigin = enemyPanel.anchoredPosition;
		
		Vector2 offset = new Vector2(400f, 0);
		Vector2 pStartPos = pPanelOrigin + offset;
		Vector2 eStartPos = ePanelOrigin - offset;
		
		//옮기기
		playerPanel.anchoredPosition = pStartPos;
		enemyPanel.anchoredPosition = eStartPos;
	}


	#region Animation

	//등장 애니메이션
	public void SpawnStatePanel(bool isPlayer)
	{
		float playTime = 0.4f;
		if (isPlayer)
			playerPanel.DOAnchorPos(pPanelOrigin, playTime).SetEase(Ease.OutQuad);
		else
			enemyPanel.DOAnchorPos(ePanelOrigin, playTime).SetEase(Ease.OutQuad);
	}

	#endregion
	
}

	