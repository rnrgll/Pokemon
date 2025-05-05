using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//베이스 클래스
public class InGameContext
{
	public bool IsInBattle;
	public bool IsInDungeon;
	public bool IsWildBattle;
	public UI_PokemonSlot PokemonSlot;
	public Action<string> NotifyMessage;

	public Action Callback;
	
	public bool Result { get; set; } // Use 결과 저장용(필요할때 쓰기)
	
	// 체이닝 가능한 메서드들
	public InGameContext SetMessage(Action<string> msg)
	{
		NotifyMessage = msg;
		return this;
	}

	public InGameContext SetCallback(Action cb)
	{
		Callback = cb;
		return this;
	}
}

	

