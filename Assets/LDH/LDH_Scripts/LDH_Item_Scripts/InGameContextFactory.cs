using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class InGameContextFactory
{
	public static InGameContext CreateBasic(
		bool isBattle, 
		bool isWild = false, 
		bool isDungeon = false, 
		Action<string> message = null, 
		Action onDone = null)
	{
		return new InGameContext
		{
			IsInBattle = isBattle,
			IsWildBattle = isWild,
			IsInDungeon = isDungeon,
			NotifyMessage = message,
			Callback = onDone
		};
	}


	//
	// public static InGameContext<T> Create<T>(
	// 	bool isBattle = false,
	// 	bool isWild = false,
	// 	bool isDungeon = false,
	// 	Action<string> message = null,
	// 	Action<T> callback = null)
	// {
	// 	return new InGameContext<T>(callback)
	// 	{
	// 		IsInBattle = isBattle,
	// 		IsWildBattle = isWild,
	// 		IsInDungeon = isDungeon,
	// 		NotifyMessage = message
	// 	};
	// }
	//
	public static InGameContext CreateFromGameManager()
	{
		return CreateBasic(
			Manager.Game.IsInBattle,
			Manager.Game.IsWildBattle,
			Manager.Game.IsInDungeon
		);
	}
}
