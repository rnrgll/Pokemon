using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/KeyItem")]
public class Item_KeyItem : ItemBase
{
	
	public override void Use(Pokemon target, InGameContext inGameContext)
    {
	    inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.CanNotUse));
    }
}
