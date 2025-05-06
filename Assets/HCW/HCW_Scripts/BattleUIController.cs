using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;

public class BattleUIController : MonoBehaviour
{
	
	[Header("액션")]
	[SerializeField] private UI_BattleMenuController UI_BattleMenu;


	
	[Header("패널")]
	public GameObject bottomMenuPanel;
	public GameObject skillPanel;

	[Header("스킬")]
	[SerializeField] private UI_BattleSkillMenuController UI_SkillMenu;
	
	
	private List<List<UI_GenericSelectButton>> menuButtonList;
	private List<UI_GenericSelectButton> skillButtonList;

	
	public UnityEvent<string> OnActionSelected = new UnityEvent<string>();
	public UnityEvent<int> OnSkillSelected = new UnityEvent<int>();

	
	private Pokémon _curPokemon;

	void Awake()
	{

		menuButtonList = UI_BattleMenu.MenuButtonGrid;
		skillButtonList = UI_SkillMenu.SkillButtonList;
		
		UI_SkillMenu.OnCanceled += () => ShowActionMenu(_curPokemon);
		UI_SkillMenu.OnCanceled += HideSkillSelection;
		
	
	}

	private void OnDestroy()
	{
		//UI_SkillMenu.OnCanceled -= ShowActionMenu;
		UI_SkillMenu.OnCanceled -= HideSkillSelection;
	}

	void Start()
	{
		// 대사창이 끝날때까지 액션 버튼 비활성화
		bottomMenuPanel.SetActive(false);
		// 기술 패널도 숨김
		skillPanel.SetActive(false);

		// 액션 버튼 이벤트 등록 => 액션 버튼 클릭 대신 키 입력으로 변경
		menuButtonList[0][0].SetAction(new CustomAction( () => ShowSkillSelection(_curPokemon)
			));
		menuButtonList[0][1].SetAction(new CustomAction(()=>OnActionSelected.Invoke("Bag")));
		menuButtonList[1][0].SetAction(new CustomAction(()=>OnActionSelected.Invoke("Pokemon")));
		menuButtonList[1][1].SetAction(new CustomAction(()=>OnActionSelected.Invoke("Run")));

		// 스킬 버튼 이벤트 등록 => //키 컨트롤용 버튼으로 수정
		for (int i = 0; i < skillButtonList.Count; i++)
		{
			int skillIndex = i;
			UI_SkillMenu.SkillButtonList[skillIndex].SetAction(new CustomAction(()=>
			{
				
				//스킬을 선택했을 때 공격으로 액션 판정
				OnSkillSelected.Invoke(skillIndex);
				OnActionSelected.Invoke("Fight");
				HideSkillSelection();
			}));
			UI_SkillMenu.SkillButtonList[skillIndex].SetArrowActive(false);
		}
		skillPanel.SetActive(false);
	}
	public void ShowActionMenu(Pokémon curPokemon)
	{
		this._curPokemon = curPokemon; // 외부에서 받은 포켓몬 저장
		bottomMenuPanel.SetActive(true);
		// EventSystem.current.SetSelectedGameObject(fightButton.gameObject);
	}


	public void HideActionMenu()
	{
		Debug.Log("액션메뉴 비활성화");
		bottomMenuPanel.SetActive(false);
		// EventSystem.current.SetSelectedGameObject(null);
	}

	

	public void ShowSkillSelection(Pokémon pokemon)
	{
		//UI에 스킬 정보 반영을 위해 UI 컨트롤러에 포켓몬 정보 넘겨주기
		//동일한 값이 아닌 경우만 넘겨주기
		if (UI_SkillMenu.Pokemon != pokemon)
			UI_SkillMenu.Pokemon = pokemon;
		
		var skills = pokemon.skills;
		for (int i = 0; i < skillButtonList.Count; i++)
		{
			// 스킬 버튼에 스킬 이름 설정 및 한칸씩 채워넣기
			var btn = skillButtonList[i];
			var txt = btn.GetComponentInChildren<TMP_Text>();
			if (skills.Count > i)
			{
				txt.text = skills[i];
				btn.gameObject.SetActive(true);
			}
			else
			{
				txt.text = "_";
				btn.SetArrowActive(false);
			}
		}
		skillPanel.SetActive(true);
		// 스킬 버튼에 하이라이트 효과 추가
		// EventSystem.current.SetSelectedGameObject(skillButton1.gameObject);
	}

	// 스킬 선택 후 스킬 패널 숨김
	public void HideSkillSelection()
	{
		skillPanel.SetActive(false);
		// EventSystem.current.SetSelectedGameObject(fightButton.gameObject);
	}
}
