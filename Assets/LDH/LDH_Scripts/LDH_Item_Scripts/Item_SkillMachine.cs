using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillMachineType {TM, HM}

[CreateAssetMenu(menuName = "Item/SkillMachine")]
public class Item_SkillMachine : ItemBase
{
	[SerializeField] protected Define.ItemCategory category; 
	[SerializeField] protected SkillMachineType machineType;
	[SerializeField] protected int machineNumber;
	[SerializeField] protected int skillID;
	
	public string SkillId => machineType.ToString();
	[SerializeField] protected List<int> pokemonIdList;
	public bool IsHM => machineType == SkillMachineType.HM;
	public bool IsTM => machineType == SkillMachineType.TM;
	public bool IsConsumable => IsTM;
	public bool IsSellable => !IsHM;

	public int MachineNumber => machineNumber;

	
	public bool CanLearn(Pokémon pokemon)
		=> pokemonIdList.Contains(pokemon.id);
	
	public override bool Use(Pokémon target, InGameContext inGameContext)
	{
		throw new System.NotImplementedException();
	}
}
