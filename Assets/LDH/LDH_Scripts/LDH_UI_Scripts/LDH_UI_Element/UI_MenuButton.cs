using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UI_MenuButton : MonoBehaviour
{
	[Header("Flag")]
	[SerializeField] private bool isOpened;
	public bool IsOpend => isOpened;
	[SerializeField] private bool isPlayerMenu;
	
	[Header("Data")]
	[SerializeField] private MenuDescriptasbleData _descriptasbleData;
	
	[Header("Component")]
	[SerializeField] private GameObject _arrowObj;
	[SerializeField] private TMP_Text _menuButtonText;
	
	[Header("Linked UI Prefab path")]
	[SerializeField] private string _linkedPrefabPath;
	private void Awake()
	{
		_arrowObj = transform.GetChild(0).gameObject;
		_menuButtonText = transform.GetChild(1).GetComponent<TMP_Text>();

		if (isPlayerMenu)
		{
			//Todo : 데이터 연동 예정
			_menuButtonText.text = "임시"; //연동 예정
		}
		else
		{
			_menuButtonText.text = _descriptasbleData.menuName;
			
		}
	}

	private void OnEnable()
	{
		RefreshState();
	}

	public void RefreshState()
	{
		gameObject.SetActive(isOpened);
		SetArrowActive(false);
	}
	
	public void SetArrowActive(bool active)
	{
		if(_arrowObj!=null)
			_arrowObj.SetActive(active);
	}

	public string GetMenuDescription()
	{
		return _descriptasbleData.description;
	}
	
}
