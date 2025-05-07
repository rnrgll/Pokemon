using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDH_Test : MonoBehaviour
{
	public BattleManager bm;
	private void Start()
	{
		Manager.Game.Player.PrevSceneName = "Route29";
	}

	public void ExpTest(int exp)
	{
		Debug.Log("<color=red> 경험치 증가 시작</color>");
		StartCoroutine(bm.AnimateGainExp(exp));
		Debug.Log("<color=red> 경험치 증가 끝</color>");
	}	
	
}
