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
			_menuButtonText.text = Manager.Data.LdhPlayerData.PlayerName;
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

	public void OpenMenu()
	{
		if (string.IsNullOrEmpty(_linkedPrefabPath))
		{
			//연결된 프리팹 경로가 없는 경우 -> 구현되지 않은 기능
			Debug.Log("구현되지 않은 기능입니다.");
			return;
		}
		Manager.UI.ShowLinkedUI<UI_Linked>(_linkedPrefabPath);
	}
	
}
