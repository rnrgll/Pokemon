using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/MonsterBall")]
public class Item_MonsterBall : ItemBase
{
	[SerializeField] private float catchRate;
	public float CatchRate => catchRate;
	public override bool Use(Pokémon target, InGameContext inGameContext)
	{
		if (!inGameContext.IsWildBattle)
		{
			inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.CanNotUseBall));
			return false;
		}

		
		inGameContext.NotifyMessage?.Invoke($"{Manager.Data.PlayerData.PlayerName}는 {itemName}을(를) 던졌다!");
	
		return true;
	}
}