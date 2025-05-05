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
	public Button fightButton;
	// public Button bagButton;
	// public Button pokemonButton;
	// public Button runButton;
	[SerializeField] private UI_BattleMenuController UI_BattleMenu;
	
	[Header("패널")]
	public GameObject bottomPanel;
	public GameObject skillPanel;

	[Header("스킬")]
	public Button skillButton1;
	public Button skillButton2;
	public Button skillButton3;
	public Button skillButton4;
	
	public UnityEvent<string> OnActionSelected = new UnityEvent<string>();
	public UnityEvent<int> OnSkillSelected = new UnityEvent<int>();

	private List<Button> skillButtons;

	void Awake()
	{
		// 스킬 버튼을 리스트로 묶어 관리
		skillButtons = new List<Button> { skillButton1, skillButton2, skillButton3, skillButton4 };
	}
	void Start()
	{
		// 대사창이 끝날때까지 액션 버튼 비활성화
		bottomPanel.SetActive(false);
		// 기술 패널도 숨김
		skillPanel.SetActive(false);

		// 액션 버튼 이벤트 등록
		//fightButton.onClick.AddListener(() => OnActionSelected.Invoke("Fight"));
		// bagButton.onClick.AddListener(() => OnActionSelected.Invoke("Bag"));
		// pokemonButton.onClick.AddListener(() => OnActionSelected.Invoke("Pokemon"));
		// runButton.onClick.AddListener(() => OnActionSelected.Invoke("Run"));
		
		//액션 버튼 클릭 대신 키 입력으로 변경
		UI_BattleMenu.MenuButtonGrid[0][0].SetAction(new CustomAction(() => OnActionSelected.Invoke("Fight")));
		UI_BattleMenu.MenuButtonGrid[0][1].SetAction(new CustomAction(()=>OnActionSelected.Invoke("Bag")));
		UI_BattleMenu.MenuButtonGrid[1][0].SetAction(new CustomAction(()=>OnActionSelected.Invoke("Pokemon")));
		UI_BattleMenu.MenuButtonGrid[1][1].SetAction(new CustomAction(()=>OnActionSelected.Invoke("Run")));

		// 스킬 버튼 이벤트 등록
		for (int i = 0; i < skillButtons.Count; i++)
		{
			int idx = i;
			skillButtons[i].onClick.AddListener(() => {OnSkillSelected.Invoke(idx); skillPanel.SetActive(false);});
		}
	}
	public void ShowActionMenu()
	{
		bottomPanel.SetActive(true);
		// EventSystem.current.SetSelectedGameObject(fightButton.gameObject);
	}


	public void HideActionMenu()
	{
		Debug.Log("액션메뉴 비활성화");
		bottomPanel.SetActive(false);
		// EventSystem.current.SetSelectedGameObject(null);
	}

	

	public void ShowSkillSelection(Pokémon pokemon)
	{
		var skills = pokemon.skills;
		for (int i = 0; i < skillButtons.Count; i++)
		{
			// 스킬 버튼에 스킬 이름 설정 및 한칸씩 채워넣기
			var btn = skillButtons[i];
			var txt = btn.GetComponentInChildren<TMP_Text>();
			if (skills.Count > i)
			{
				txt.text = skills[i];
				btn.gameObject.SetActive(true);
			}
			else
			{
				btn.gameObject.SetActive(false);
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
