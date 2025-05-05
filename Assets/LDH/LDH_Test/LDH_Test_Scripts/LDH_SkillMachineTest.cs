using System;
using UnityEngine;

namespace LDH.LDH_Scripts
{
	public class LDH_SkillMachineTest : MonoBehaviour
	{
		[SerializeField] private Item_SkillMachine _skillMachine;

		[SerializeField] private Pokémon pokemon;
		[SerializeField] private bool isInBattle;
		private InGameContext inGameContext;

		public string[] tempAddSkill;
		
		private void Start()
		{
			pokemon.Init(4, 10);

			inGameContext = new InGameContext
			{
				Callback = Manager.UI.UndoLinkedUI,
				IsInBattle = isInBattle,
				IsInDungeon = false,
				NotifyMessage = msg => Debug.Log(msg)
			};

			// if (tempAddSkill.Length != 0)
			// {
			// 	foreach (string skill in tempAddSkill)
			// 	{
			// 		pokemon.TempSkillAdd(skill);
			// 	}
			// }

			PrintSkill();
			
		}
		
		public void UseItem()
		{
			Manager.UI.ShowLinkedUI<UI_Menu>("UI_Menu");
			 ItemBase item = _skillMachine;
			Debug.Log($"대상이 필요? :  {item.RequiresTarget()}");
	    
			Debug.Log($"지금 사용가능? :  {item.CanUseNow(inGameContext)}");
			
			bool issuccess =  item.Use(pokemon,inGameContext);
			Debug.Log(issuccess);
			PrintSkill();
	    
			// item.Use(null,inGameContext);
		}


		private void PrintSkill()
		{
			foreach (string skill in pokemon.skills)
			{
				Debug.Log($"현재 보유 스킬 : {skill}");
			}
		}
	}
}