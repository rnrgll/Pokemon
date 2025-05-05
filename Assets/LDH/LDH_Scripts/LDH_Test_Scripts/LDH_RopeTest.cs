using System;
using UnityEngine;


public class LDH_RopeTest : MonoBehaviour
{
	// [SerializeField] private Item_EscapeRope _escapeRope;
	// [SerializeField] private bool isInDungeon;
	//
	//
	// public void UseRope()
	// {
	// 	var itemContext = InGameContextFactory.Create<DungeonMapData.DungeonLink>(
	// 		isDungeon: isInDungeon,
	// 		message: msg => Debug.Log(msg),
	// 		callback: info =>
	// 		{
	// 			SceneChanger sc = new GameObject().AddComponent<SceneChanger>();
	// 			sc.Change(info.entranceSceneName, info.entrancePosition);
	// 		});
	//
	// 	_escapeRope.Use<DungeonMapData.DungeonLink>(null, itemContext);
	// }

	public bool iswild;
	private void Start()
	{
		Manager.Game.SetDungeonState(true);
		
		Manager.Poke.AddEnemyPokemon("치코리타", 20);
		Manager.Game.SetBattleState(false,iswild,Manager.Game.EnemyPokemon);

	}
}
