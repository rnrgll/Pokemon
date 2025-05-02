using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/KeyItem")]
public class Item_KeyItem : ItemBase
{
	
	public override bool Use(Pokémon target, InGameContext inGameContext)
    {
	    
	    inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.CanNotUse,Manager.Data.PlayerData.PlayerName));

	    return false;
    }
}
