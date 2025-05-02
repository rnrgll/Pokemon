using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public enum SkillMachineType {TM, HM}

[CreateAssetMenu(menuName = "Item/SkillMachine")]
public class Item_SkillMachine : ItemBase
{
	[SerializeField] protected SkillMachineType machineType; //머신 타입(기술머신 or 비전머신)
	[SerializeField] protected int machineNumber; //머신  넘버
	[SerializeField, ReadOnly] private string skillCode; //머신 코드(머신 넘버를 입력하면 TMnn HMnn으로 자동 생성)
	[SerializeField] private string skillName;
	
	public string Description
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

	public string SkillCode => skillCode;
	[SerializeField] protected List<int> pokemonIdList;
	public int MachineNumber => machineNumber;
	public bool CanLearn(Pokémon pokemon)
		=> pokemonIdList.Contains(pokemon.id);
	
	
	public override bool Use(Pokémon target, InGameContext inGameContext)
	{
		//해당 기술을 배울 수 있는 포켓몬인지 확인
		if (!CanLearn(target))
		{
			string message = $"{target.pokeName}과(와) {skillName}는(은) 상성이 좋지 않았다!\n{skillName}은(는) 배울 수 없다!";
			
			inGameContext.NotifyMessage?.Invoke(message);
			inGameContext.Callback?.Invoke();
			return false;
		}
		bool isSuccess = target.TryLearnSkill(skillName);
		inGameContext.NotifyMessage?.Invoke(isSuccess.ToString());
		inGameContext.Callback?.Invoke();
		return isSuccess;
	}
	
#if UNITY_EDITOR
	private void OnValidate()
	{
		skillCode = $"{machineType}{machineNumber:D2}";
	}
#endif
	
}
