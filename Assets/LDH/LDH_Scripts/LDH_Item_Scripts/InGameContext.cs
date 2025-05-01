using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InGameContext
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
}
