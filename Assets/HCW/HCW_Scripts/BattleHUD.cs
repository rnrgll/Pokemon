using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
	[SerializeField] private TMP_Text playerNameText;
	[SerializeField] private TMP_Text playerLevelText;
	[SerializeField] private Slider playerHPBar;
	[SerializeField] private TMP_Text playerHPText;
	// 추후 상태표시창 추가 가능

	[SerializeField] private TMP_Text enemyNameText;
	[SerializeField] private TMP_Text enemyLevelText;
	[SerializeField] private Slider enemyHPBar;
	[SerializeField] private TMP_Text enemyHPText;
	// 추후 상태 표시창 추가가능

	public void SetPlayerHUD(Pokemon p)
	{
		playerNameText.text = p.Name;            // 이름
		playerLevelText.text = $"Lv {p.Level}";  // 레벨
		playerHPBar.maxValue = p.MaxHP;          // 최대 HP
		playerHPBar.value = p.HP;                // 현재 HP
		playerHPText.text = $"{p.HP}/{p.MaxHP}"; // 숫자 표시
		// 추후 상태 표시창 추가가능
	}

	public void SetEnemyHUD(Pokemon e)
	{
		enemyNameText.text = e.Name;
		enemyLevelText.text = $"Lv {e.Level}";
		enemyHPBar.maxValue = e.MaxHP;
		enemyHPBar.value = e.HP;
		enemyHPText.text = $"{e.HP}/{e.MaxHP}";
		// 추후 상태 표시창 추가가능
	}
}
