using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PokemonSkillInfo : UI_Linked
{
	public List<Pokémon> party;
	public int partyIdx;
	public Pokémon selectedPokemon;

	[SerializeField] private int curIdx;

	[Header("상단 화살표 ui")]
	[SerializeField] private GameObject leftArrow;
	[SerializeField] private GameObject rightArrow;

	[Header("포켓몬 정보 ui")] [SerializeField]
	private Image pokemonImg;

	[SerializeField] private TMP_Text pokemonName;
	[SerializeField] private TMP_Text levelText;
	[SerializeField] private TMP_Text curHpText;

	[SerializeField] private TMP_Text maxHpText;
	
	
	[Header("스킬 슬롯 리스트")]
	[SerializeField] private Transform[] skillSlotList;

	[Header("스킬 상세 정보 ui")]
	[SerializeField] private TMP_Text typeText;
	[SerializeField] private TMP_Text damageText;
	[SerializeField] private TMP_Text description;

	private int totalSkillCnt;
	private List<string> skillList;
	private List<SkillS> cachedSkillInfoList; // 💡 캐싱된 스킬 데이터

	private void OnEnable()
	{
		party = Manager.Poke.party;
		curIdx = 0;
		UpdateSkillListUI();
		UpdateSkillDetailUI();
	}

	public override void HandleInput(Define.UIInputType inputType)
	{
		switch (inputType)
		{
			case Define.UIInputType.Left:
				partyIdx = Mathf.Clamp(partyIdx - 1, 0, party.Count - 1);
				curIdx = 0;
				UpdateSkillListUI();
				UpdateSkillDetailUI();
				break;

			case Define.UIInputType.Right:
				partyIdx = Mathf.Clamp(partyIdx + 1, 0, party.Count - 1);
				curIdx = 0;
				UpdateSkillListUI();
				UpdateSkillDetailUI();
				break;

			case Define.UIInputType.Up:
				curIdx = Mathf.Clamp(curIdx - 1, 0, totalSkillCnt - 1);
				UpdateSkillDetailUI();
				break;

			case Define.UIInputType.Down:
				curIdx = Mathf.Clamp(curIdx + 1, 0, totalSkillCnt - 1);
				UpdateSkillDetailUI();
				break;

			case Define.UIInputType.Select:
				OnSelect();
				break;

			case Define.UIInputType.Cancel:
				OnCancel();
				break;
		}
	}

	private void UpdateSkillListUI()
	{
		//상단 화살표
		leftArrow.SetActive(partyIdx > 0);
		rightArrow.SetActive(partyIdx < party.Count - 1);
		
		//포켓몬 캐싱
		selectedPokemon = party[partyIdx];
		
		skillList = selectedPokemon.skills;
		totalSkillCnt = skillList.Count;
		
		
		//포켓몬 슬롯도 업데이트
		//todo : 이미지는 일단 패스
		pokemonName.text = selectedPokemon.pokeName;
		levelText.text = $":L{selectedPokemon.level}";
		curHpText.text = $"{selectedPokemon.hp}/";
		maxHpText.text = selectedPokemon.maxHp.ToString();

		// 캐싱
		cachedSkillInfoList = new List<SkillS>();
		for (int i = 0; i < totalSkillCnt; i++)
		{
			var skillData = Manager.Data.SkillSData.GetSkillDataByName(skillList[i]);
			cachedSkillInfoList.Add(skillData);
		}

		// 슬롯 UI 설정
		for (int i = 0; i < 4; i++)
		{
			bool hasSkill = i < totalSkillCnt;
			SetSkillUI(skillSlotList[i],i, hasSkill, hasSkill ? cachedSkillInfoList[i] : null);
		}
	}

	private void UpdateSkillDetailUI()
	{
		SkillS skill = null;

		if (curIdx >= 0 && curIdx < cachedSkillInfoList.Count)
			skill = cachedSkillInfoList[curIdx];

		if (skill != null)
		{
			//typeText.text = skill.type.ToString();
			typeText.text = Define.GetKoreanPokeType[skill.type];
			damageText.text = skill.damage.ToString();
			description.text = skill.description;
		}
		else
		{
			typeText.text = "-";
			damageText.text = "-";
			description.text = "정보 없음";
		}

		// 화살표 갱신: 스킬 유무와 관계없이 항상 작동
		for (int i = 0; i < 4; i++)
		{
			bool isSelected = (curIdx == i);
			var arrow = skillSlotList[i].GetChild(0).GetComponent<CanvasGroup>();
			Util.SetVisible(arrow, isSelected);
		}
	}


	private void SetSkillUI(Transform slot, int index, bool hasSkill, SkillS data)
	{
		bool isSelected = (curIdx == index);

		// 화살표 표시
		Util.SetVisible(slot.GetChild(0).GetComponent<CanvasGroup>(), isSelected);

		string nameText = "_";
		string ppText = "_";
		string curPP = "";
		string maxPP = "";

		if (hasSkill && data != null)
		{
			SkillData skillData = party[partyIdx].skillDatas[index];
			nameText = skillData.Name;
			ppText = "PP";
			curPP = $"{skillData.CurPP}/";
			maxPP = skillData.MaxPP.ToString();

			//nameText = data.name;
			//ppText = "PP";
			//curPP = $"{data.curPP}/";
			//maxPP = data.maxPP.ToString();
		}
		else if (hasSkill && data == null)
		{
			nameText = "(정보 없음)";
		}

		slot.GetChild(1).GetComponent<TMP_Text>().text = nameText;
		slot.GetChild(2).GetComponent<TMP_Text>().text = ppText;
		slot.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = curPP;
		slot.GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = maxPP;
	}


	public void SetPartyIdx(int partyIdx)
	{
		this.partyIdx = partyIdx;
	}
}
