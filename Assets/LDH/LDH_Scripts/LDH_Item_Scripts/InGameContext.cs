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
	public Action<string> NotifyMessage;
	public Action Callback;
	
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



// InGameContext의 확장형 버전으로, 특정 타입의 콜백(Action<T>)을 함께 전달할 수 있도록 만든 클래스
// 기본 게임 상태 정보 외에도 아이템 사용 등에서 외부 처리 요청이 필요한 경우 사용
// public class InGameContext<T> : InGameContext 
// {
// 	public new Action<T> Callback;
//
// 	public InGameContext(Action<T> callback)
// 	{
// 		Callback = callback;
// 	}
// }
	

