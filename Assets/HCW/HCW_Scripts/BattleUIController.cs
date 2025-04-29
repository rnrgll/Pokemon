using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

	public UnintyEvent<string> OnActionSelected = new UnityEvent<string>();
	public UnityEvent<int> OnSkillSelected = new UnityEvent<int>();

	void Start()
	{
		// 스킬패널 기본 비활성화
		skillPanel.SetActive(false);
		// 메인 액션 버튼들
		fightButton.onClick.AddListener(OnFightButtonClicked);
		bagButton.onClick.AddListener(() => Debug.Log("BAG 눌림"));
		pokemonButton.onClick.AddListener(() => Debug.Log("POKEMON 눌림"));
		runButton.onClick.AddListener(() => Debug.Log("RUN 눌림"));

		// 스킬 버튼들 
		skillButton1.onClick.AddListener(() => OnSkillButtonClicked(1)); 
		skillButton2.onClick.AddListener(() => OnSkillButtonClicked(2));
		skillButton3.onClick.AddListener(() => OnSkillButtonClicked(3));
		skillButton4.onClick.AddListener(() => OnSkillButtonClicked(4));
	}

	private void OnFightButtonClicked()
	{
		// 싸우기 버튼 클릭 시 스킬 선택창 열기
		OnActionSelected.Invoke("fight"); // 싸우기 액션 선택 이벤트 호출
		skillPanel.SetActive(true);
		Debug.Log("FIGHT 눌림 → 스킬 선택창 열기");
	}

	private void OnSkillButtonClicked(int skillNumber)
	{
		OnSkillButtonClicked(skillNumber); // 스킬 선택 이벤트 호출
		Debug.Log($"Skill {skillNumber} 선택!");
		skillPanel.SetActive(false); // 스킬 선택했으면 다시 스킬창 끄기
	}

}
