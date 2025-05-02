using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Item/EscapeRope")]
public class Item_EscapeRope : ItemBase
{
 
	public override bool Use<T>(Pokémon target, InGameContext<T> inGameContext)
    {
	    if (!inGameContext.IsInDungeon)
	    {
		    inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.CanNotUse, Manager.Data.LdhPlayerData.PlayerName));
		    return false;
	    }
		
		//todo: 탈출 처리 로직
		if (Manager.Data.DungeonMapData.TryGetLink(SceneManager.GetActiveScene().name,
				out var dungeonLink))
		{
			// string nextSceneName = dungeonLink.entranceSceneName;
			// Vector2 position = dungeonLink.entrancePosition;
			if (inGameContext is InGameContext<DungeonMapData.DungeonLink> dungeonContext)
			{
				dungeonContext.Callback?.Invoke(dungeonLink);
				return true;
			}

			Debug.LogWarning($"{nameof(Item_EscapeRope)}: InGameContext 타입이 DungeonLink와 일치하지 않습니다.");
			return false;


		}

		return false;
    }

    public override bool Use(Pokémon target, InGameContext inGameContext)
    {
	    //구현 안하는 부분 예외처리
	    return false;
    }
}
