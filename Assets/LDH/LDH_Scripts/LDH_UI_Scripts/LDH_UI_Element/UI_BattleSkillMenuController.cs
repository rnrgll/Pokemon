using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UI_BattleSkillMenuController : MonoBehaviour
{
	
	[Header("Current Pokemon")]
	//현재 포켓몬 정보
	private Pokémon pokemon;
	public Pokémon Pokemon
	{
		get => pokemon;
		set
		{
			pokemon = value;
		}
	}

	[Header("UI - Skill Button")]
	[SerializeField] private Transform buttonRoot;
	[SerializeField] private List<UI_GenericSelectButton> skillButtonList;
	public List<UI_GenericSelectButton> SkillButtonList => skillButtonList;
	
	[Header("UI - Skill Info")]
	[SerializeField] private TMP_Text skillPP;
 	[SerializeField]private TMP_Text skillType;

    
	//현재 커서 위치
	private int curX;

	
	//콞백
	public event Action OnCanceled; // 스킬 창 닫기 시
    private void OnEnable()
    {
        curX = 0;
        skillButtonList[curX].SetArrowActive(true);
        
        
    }

    private void Update()
    {
        if(Manager.UI.IsAnyUIOpen) return;
        
        if (Input.GetKeyDown(KeyCode.UpArrow)) MoveCursor(0);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) MoveCursor(1);
        else if(Input.GetKeyDown(KeyCode.Z)) OnSelect();
        else if (Input.GetKeyDown(KeyCode.X)) OnCancel();

    }

    private void MoveCursor(int dx)
    {
	    int skillCount = pokemon.skills.Count;
	    int x = curX + dx;
	    if (x < 0) x = skillCount - 1;
	    else if (x >= skillCount) x = 0;
	    
	    
        skillButtonList[curX].SetArrowActive(false);
        curX = x;
        skillButtonList[curX].SetArrowActive(true);
        UpdateSkillInfo();
        
        
    }

    private void UpdateSkillInfo()
    {
		//todo : 포켓몬 pp값 보유하게 한 후 수정 필요. 임시로 스킬 클래스 pp 반영
		SkillData skillData = pokemon.skillDatas[curX];
		string skillName = skillData.Name;
	    SkillS skill = Manager.Data.SkillSData.GetSkillDataByName(skillName);

		int curPP = skillData.CurPP;
		int maxPP = skillData.MaxPP;

	    skillPP.text = $"{curPP}/{maxPP}";
	    skillType.text = skill.skillType.ToString();
    }
    
    
    
    
    //z 키 입력시 호출되는 메소드
    public void OnSelect()
    {
        skillButtonList[curX].Trigger();
    }

    //x키 입력시 호출되는 메소드
    public void OnCancel()
    {
	    OnCanceled?.Invoke();
    }
    
}
