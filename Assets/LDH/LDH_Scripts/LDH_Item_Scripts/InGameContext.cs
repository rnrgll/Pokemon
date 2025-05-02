using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//베이스 클래스
public class InGameContext
{
	public bool IsInBattle;
	public bool IsInDungeon;
	public Action<string> NotifyMessage;
	public Action Callback;
}

// InGameContext의 확장형 버전으로, 특정 타입의 콜백(Action<T>)을 함께 전달할 수 있도록 만든 클래스
// 기본 게임 상태 정보 외에도 아이템 사용 등에서 외부 처리 요청이 필요한 경우 사용
public class InGameContext<T> : InGameContext 
{
	//현재 배틀 중인지 여부
	public bool IsInBattle;
	
	//배틀 대상이 야생 포켓몬인지 여부 (몬스터볼 용도)
	public bool IsWildBattle;
	
	// 현재 위치가 던전이나 동굴인지 여부 (동굴탈출로프 용도 등)
	public bool IsInDungeon;
	
	/// 아이템 효과나 메시지를 전달할 UI/피드백 시스템
	//public IItemFeedbackHandler Feedback;


	public Action<string> NotifyMessage;
	public Action<T> Callback; //콜백함수
	
	//생성자
	public InGameContext(Action<T> callback)
	{
		Callback = callback;
	}

	//팩토리 메서드
	public static InGameContext<T> With(Action<T> callback)
	{
		return new InGameContext<T>(callback);
	}
	
	
	
}
