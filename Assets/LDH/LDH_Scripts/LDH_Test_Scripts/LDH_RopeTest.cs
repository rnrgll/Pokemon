using UnityEngine;


public class LDH_RopeTest : MonoBehaviour
{
	[SerializeField] private Item_EscapeRope _escapeRope;
	[SerializeField] private bool isInDungeon;
	
	
	public void UseRope()
	{
		InGameContext<DungeonMapData.DungeonLink> itemContext = InGameContext<DungeonMapData.DungeonLink>.With(info =>
		{
			SceneChanger sc = new GameObject().AddComponent<SceneChanger>();
			sc.Change(info.entranceSceneName, info.entrancePosition);
		});
		itemContext.IsInDungeon = isInDungeon;
		itemContext.NotifyMessage = msg => Debug.Log(msg);
		_escapeRope.Use<DungeonMapData.DungeonLink>(null, itemContext);
	}
}
