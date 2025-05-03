using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIMenuSelectButton : UI_GenericSelectButton
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

}

