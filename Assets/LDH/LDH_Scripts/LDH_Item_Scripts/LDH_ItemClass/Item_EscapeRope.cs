using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Item/EscapeRope")]
public class Item_EscapeRope : ItemBase
{
    public override bool Use(Pokémon target, InGameContext inGameContext)
    {
	    if (!inGameContext.IsInDungeon)
	    {
		    inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.CanNotUse, Manager.Data.PlayerData.PlayerName));
		    return false;
	    }
	    
	    if(Manager.Data.DungeonMapData.TryGetLink(SceneManager.GetActiveScene().name,
		    out var dungeonLink))
	    {
		    Manager.UI.CloseAllUI();
		    Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp")
			    .ShowMessage(new List<string> { $"{Manager.Data.PlayerData.PlayerName}는(은) {itemName}을 사용했다!" }, 
				    () =>
			    {
				    EscapeDungeon(dungeonLink);
			    },
			    false
				    );
		    inGameContext.Callback?.Invoke();
		    return true;
	    }

	    return false;
    }

    private void EscapeDungeon(DungeonMapData.DungeonLink dungeonLink)
    {
	    //테스트코드
	    SceneChanger sc = new GameObject().AddComponent<SceneChanger>();
	    sc.Change(dungeonLink.entranceSceneName, dungeonLink.entrancePosition);
    }
}
