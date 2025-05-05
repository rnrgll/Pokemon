using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum SkillMachineType {TM, HM}

[CreateAssetMenu(menuName = "Item/SkillMachine")]
public class Item_SkillMachine : ItemBase
{
	[SerializeField] protected SkillMachineType machineType; //머신 타입(기술머신 or 비전머신)
	[SerializeField] protected int machineNumber; //머신  넘버
	[SerializeField, ReadOnly] private string skillCode; //머신 코드(머신 넘버를 입력하면 TMnn HMnn으로 자동 생성)
	[SerializeField] private string skillName;
	
	public override string Description
	{
		get
		{
			// SkillSData에서 skillId로 기술 설명 조회
			var skill = Manager.Data.SkillSData.GetSkillDataByName(skillName);
			if (skill != null)
			{
				description = skill.description;
			}
			else
			{
				description = "스킬 설명이 없음";
			}

			return description;
		}
	}

	public SkillMachineType MachineType => machineType;
	public string SkillCode
	{
		get
		{
			if (machineType == SkillMachineType.HM)
				return $"비{machineNumber:D1}";
			else
				return $"{machineNumber:D2}";
		}
	}
	
	[SerializeField] protected List<int> pokemonIdList;
	public int MachineNumber => machineNumber;
	public bool CanLearn(Pokémon pokemon)
		=> pokemonIdList.Contains(pokemon.id);
	
	
	public override bool Use(Pokémon target, InGameContext inGameContext)
	{
		//해당 기술을 배울 수 있는 포켓몬인지 확인
		 if (!CanLearn(target))
		 {
		 	Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp").ShowMessage(ItemMessage.Get(ItemMessageKey.CanNotLearn,target.pokeName,skillName),Manager.UI.UndoLinkedUI,true,true);
		 	// inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.CanNotLearn,target.pokeName,skillName));
		 	inGameContext.Callback?.Invoke();
		 	return false;
		 }
		 


		 target.TryLearnSkill(skillName, (bool isSuccess) =>
		 {
			 inGameContext.Result = isSuccess;
			 inGameContext.Callback?.Invoke();
		 });

		return false; //일단 무조건 false 반환
		//기술머신 아이템만 콜백으로 결과 값이용한다!

	}
}
