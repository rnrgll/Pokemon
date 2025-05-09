using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleChanger : MonoBehaviour
{
	void Start()
	{
		Debug.Log("Z 키를 눌러주세요");
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			Debug.Log("씬전환 : 타이틀 > 주인공집 2층");
			SceneManager.LoadScene("PlayerHouse2F");
		}
	}
}
