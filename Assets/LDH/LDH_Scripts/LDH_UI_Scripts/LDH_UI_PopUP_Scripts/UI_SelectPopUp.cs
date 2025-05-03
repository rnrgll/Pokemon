using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_SelectPopUp : UI_PopUp
{
	
	private List<UI_GenericSelectButton> selectList = new ();
	
	private int _preIdx = 0;
	private int _curIdx = 0;
	public Transform buttonParent;

	protected void Awake()
	{
		foreach (Transform child in buttonParent)
		{
			var button = child.GetComponent<UI_GenericSelectButton>();
			button.SetArrowActive(false);
			selectList.Add(button);
			
		}
	}

	private void OnEnable()
	{
		_curIdx = 0;
		UpdateArrow();
	}

	public override void HandleInput(Define.UIInputType inputType)
	{

		switch (inputType)
		{
			case Define.UIInputType.Up:
				MoveIdx(-1);
				break;
			case Define.UIInputType.Down:
				MoveIdx(1);
				break;
			case Define.UIInputType.Select:
				OnSelect();
				break;
			case Define.UIInputType.Cancel:
				OnCancle();
				break;
		}
	}


	void MoveIdx(int direction)
	{
		_preIdx = _curIdx;
		_curIdx += direction;
		if (_curIdx < 0)
			_curIdx = selectList.Count - 1;
		else if (_curIdx >= selectList.Count)
			_curIdx = 0;
        
		UpdateArrow();
	}


	void UpdateArrow()
	{ 
		selectList[_preIdx].SetArrowActive(false);
		selectList[_curIdx].SetArrowActive(true);
	}

	public override void OnSelect()
	{
		selectList[_curIdx].Trigger();
		base.OnSelect();
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
