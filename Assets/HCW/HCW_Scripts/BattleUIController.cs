using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIController : MonoBehaviour
{
	public Button fightButton;
	public Button bagButton;
	public Button pokemonButton;
	public Button runButton;

	public GameObject bottomPanel;
	public GameObject skillPanel;

	public Button skillButton1;
	public Button skillButton2;
	public Button skillButton3;
	public Button skillButton4;

	void Start()
	{
		skillPanel.SetActive(false);
		fightButton.onClick.AddListener(OnFightButtonClicked);
		bagButton.onClick.AddListener(() => Debug.Log("BAG 눌림"));
		pokemonButton.onClick.AddListener(() => Debug.Log("POKEMON 눌림"));
		runButton.onClick.AddListener(() => Debug.Log("RUN 눌림"));

		skillButton1.onClick.AddListener(() => OnSkillButtonClicked(1)); 
		skillButton2.onClick.AddListener(() => OnSkillButtonClicked(2));
		skillButton3.onClick.AddListener(() => OnSkillButtonClicked(3));
		skillButton4.onClick.AddListener(() => OnSkillButtonClicked(4));
	}

	private void OnFightButtonClicked()
	{
		skillPanel.SetActive(true);
		Debug.Log("FIGHT 눌림 → 스킬 선택창 열기");
	}

	private void OnSkillButtonClicked(int skillNumber)
	{
		Debug.Log($"Skill {skillNumber} 선택!");

		skillPanel.SetActive(false); // 스킬 선택했으면 다시 스킬창 끄기
	}
}
