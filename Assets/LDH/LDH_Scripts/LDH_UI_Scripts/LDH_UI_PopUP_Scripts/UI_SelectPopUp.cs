using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_SelectPopUp : UI_PopUp
{
	
	
	private int _preIdx = 0;
	private int _curIdx = 0;
	

	protected void Awake()
	{
		foreach (Transform child in buttonParent)
		{
			var button = child.GetComponent<UI_GenericSelectButton>();
			button.SetArrowActive(false);
			_buttonList.Add(button);
			
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
				OnCancel();
				break;
		}
	}


	void MoveIdx(int direction)
	{
		_preIdx = _curIdx;
		_curIdx += direction;
		if (_curIdx < 0)
			_curIdx = _buttonList.Count - 1;
		else if (_curIdx >= _buttonList.Count)
			_curIdx = 0;
        
		UpdateArrow();
	}


	void UpdateArrow()
	{ 
		if (_buttonList.Count == 0) return;
		_buttonList[_preIdx].SetArrowActive(false);
		_buttonList[_curIdx].SetArrowActive(true);
	}

	public override void OnSelect()
	{
		base.OnSelect();
		_buttonList[_curIdx].Trigger();

	}
	
	
	public override void SetupOptions(List<(string label, ISelectableAction action)> options)
	{
		base.SetupOptions(options);
		_curIdx = 0;
		_preIdx = 0;
		UpdateArrow();
	}
	
}
