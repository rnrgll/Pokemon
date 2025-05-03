using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_MenuButton : UI_GenericButton
{
	[SerializeField] private bool isOpened;
	[SerializeField] private bool isPlayerMenu;
	[SerializeField] private MenuDescriptasbleData _descriptasbleData;
	[SerializeField] private string _linkedPrefabPath;

	public bool IsOpened => isOpened;

	protected override void Awake()
	{
		base.Awake();

		if (isPlayerMenu)
			_label.text = Manager.Data.PlayerData.PlayerName;
		else
			_label.text = _descriptasbleData.menuName;
	}
	

	public string GetMenuDescription()
	{
		return _descriptasbleData.description;
	}
	
	

	// public override void OnSelect()
	// {
	// 	if (_onSelectAction != null)
	// 	{
	// 		_onSelectAction.Invoke(); // 외부에서 설정한 델리게이트 우선
	// 	}
	// 	else if (!string.IsNullOrEmpty(_linkedPrefabPath))
	// 	{
	// 		Manager.UI.ShowLinkedUI<UI_Linked>(_linkedPrefabPath); // 메뉴 전용 기능
	// 	}
	// 	else
	// 	{
	// 		Debug.Log("기능이 구현되지 않았습니다.");
	// 	}
	// }
}

