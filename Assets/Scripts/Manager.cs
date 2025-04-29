using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Manager
{
	//사용하는 매니저 등록
	public static UIManager UI { get { return UIManager.GetInstance(); } }
	public static DataManager Data { get { return DataManager.GetInstance(); } }
	//씬 매니저, 다이얼로그 매니저, 게임 매니저 등.. 을 생성해서 등록
	
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
		//각종 매니저를 생성
		UIManager.CreateInstance();
		DataManager.CreateInstance();
	}
}
