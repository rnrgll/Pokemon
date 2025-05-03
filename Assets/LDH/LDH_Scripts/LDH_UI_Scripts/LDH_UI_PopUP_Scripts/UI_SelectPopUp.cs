using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_SelectPopUp : UI_PopUp
{
	
	private List<UI_MenuButton> selectList;
	private int preIdx;
	private int curIdx;

	private void Awake()
	{
		foreach (Transform child in transform)
		{
			selectList.Add(child.GetComponent<UI_MenuButton>());
		}
	}

	private void OnEnable()
	{
		curIdx = 0;
		UpdateArrow();
	}
	

	private void Start()
	{
		
	}
	
	void UpdateArrow()
	{ 
		selectList[preIdx].SetArrowActive(false);
		selectList[curIdx].SetArrowActive(true);
	}

	// private Dialog dialog;
	//
	//
	// public void SetDialog(string message)
	// {
	// 	string[] lines = message.Split('\n');
	// 	foreach (string line in lines)
	// 	{
	// 		dialog.Lines.Add(line);
	// 	}
	// }
}
//
//
// // UI_SelectPopUp.cs
// public class UI_SelectPopUp : UI_PopUp
// {
// 	private List<UI_GenericButton> selectList = new();
// 	private int preIdx;
// 	private int curIdx;
//
// 	private void Awake()
// 	{
// 		foreach (Transform child in transform)
// 		{
// 			var button = child.GetComponent<UI_GenericButton>();
// 			if (button != null)
// 				selectList.Add(button);
// 		}
// 	}
//
// 	private void OnEnable()
// 	{
// 		curIdx = 0;
// 		UpdateArrow();
// 	}
//
// 	private void Update()
// 	{
// 		if (Input.GetKeyDown(KeyCode.UpArrow)) MoveCursor(-1);
// 		else if (Input.GetKeyDown(KeyCode.DownArrow)) MoveCursor(1);
// 		else if (Input.GetKeyDown(KeyCode.Z)) selectList[curIdx].Trigger();
// 	}
//
// 	void MoveCursor(int dir)
// 	{
// 		preIdx = curIdx;
// 		curIdx = (curIdx + dir + selectList.Count) % selectList.Count;
// 		UpdateArrow();
// 	}
//
// 	void UpdateArrow()
// 	{
// 		selectList[preIdx].SetArrowActive(false);
// 		selectList[curIdx].SetArrowActive(true);
// 	}
//
// 	// 외부에서 버튼 세팅할 수 있도록 API 제공
// 	public void SetupOptions(List<(string label, IMenuAction action)> options)
// 	{
// 		for (int i = 0; i < options.Count; i++)
// 		{
// 			var button = selectList[i];
// 			button.Init(options[i].label, options[i].action);
// 			button.gameObject.SetActive(true);
// 		}
//
// 		// 나머지 비활성화
// 		for (int i = options.Count; i < selectList.Count; i++)
// 		{
// 			selectList[i].gameObject.SetActive(false);
// 		}
// 	}
// }
