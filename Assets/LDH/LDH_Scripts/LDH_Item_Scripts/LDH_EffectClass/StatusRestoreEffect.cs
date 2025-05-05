using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/StatusRestore")]
public class StatusRestoreEffect :  ScriptableObject, IItemEffect
{
	[SerializeField] private Define.StatusCondition targetCondition;

	public bool Apply(Pokémon target, InGameContext inGameContext)
	{
		
		if (target.condition != targetCondition)
		{
			Debug.Log($"{target.condition.ToString()} != {targetCondition.ToString()}");
			inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.NoEffect));
			return false;
		}

		if (target.RestoreStatus(targetCondition))
		{
			//갱신
			if (inGameContext.PokemonSlot != null)
			{

			}
			switch (targetCondition)
			{
				case Define.StatusCondition.Poison:
					inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.RestorePosition,target.pokeName));
					break;
				case Define.StatusCondition.Burn:
					inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.RestoreBurn,target.pokeName));
					break;
				case Define.StatusCondition.Freeze:
					inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.RestoreFreeze,target.pokeName));
					break;
				case Define.StatusCondition.Sleep:
					inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.RestoreSleep,target.pokeName));
					break;
				case Define.StatusCondition.Paralysis:
					inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.RestoreParalysis,target.pokeName));
					break;
				case Define.StatusCondition.Confusion:
					inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.RestoreConfusion,target.pokeName));
					break;
			    
			}

			return true;
		}

		return false;

	}
}