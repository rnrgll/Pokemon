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

	public void SetPlayerHUD(Pokémon p)
	{
		playerNameText.text = p.pokeName;            // 이름
		playerLevelText.text = $"Lv {p.level}";  // 레벨
		playerHPBar.maxValue = p.maxHp;          // 최대 HP
		playerHPBar.value = p.hp;                // 현재 HP
		playerHPText.text = $"{p.hp}/{p.maxHp}"; // 숫자 표시

		//Debug.Log($"{p.pokeName} : Lv. {p.level} [{p.hp} / {p.maxHp}]");

		// 추후 상태 표시창 추가가능
	}

	public void SetEnemyHUD(Pokémon e)
	{
		enemyNameText.text = e.pokeName;
		enemyLevelText.text = $"Lv {e.level}";
		enemyHPBar.maxValue = e.maxHp;
		enemyHPBar.value = e.hp;
		enemyHPText.text = $"{e.hp}/{e.maxHp}";

		//Debug.Log($"{e.pokeName} : Lv. {e.level} [{e.hp} / {e.maxHp}]");
		// 추후 상태 표시창 추가가능
	}
}
