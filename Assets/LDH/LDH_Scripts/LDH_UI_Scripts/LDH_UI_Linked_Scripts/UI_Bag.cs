using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static Define;
public class UI_Bag : UI_Linked
{
	private static BagPanel currentPanel = BagPanel.Items;

	[Header("UI 오브젝트 참조")]
	[SerializeField] private Image bagIconImg;
	[SerializeField] private Image labelImg;
	[SerializeField] private GameObject itemList;

	[Header("리소스")] 
	[SerializeField] private Sprite[] bagIconSprites;
	[SerializeField] private Sprite[] labelSprites;

	
	private void OnEnable()
	{
		UpdateUI();
	}

	protected override void Init()
	{
		base.Init();
		UpdateUI();
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			MovePanel(-1);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			MovePanel(1);
		}
	}
	
	
	void MovePanel(int direction)
	{
		int next = (int)currentPanel + direction;
		int max = (int)Define.BagPanel.Count;

		if (next < 0) next = max - 1;
		else if (next >= max) next = 0;

		currentPanel = (Define.BagPanel)next;

		UpdateUI();
	}

	void UpdateUI()
	{
		UpdateBagIcon(currentPanel);
		UpdateTagImage(currentPanel);
	}



	private void UpdateBagIcon(Define.BagPanel panel)
	{
		bagIconImg.sprite = bagIconSprites[(int)panel];
	}

	private void UpdateTagImage(Define.BagPanel panel)
	{
		labelImg.sprite = labelSprites[(int)panel];
	}
	//private void UpdateItemList(Define.BagPanel panelType) { /* ... */ }


	
	
	
	
	
	
    //해당 UI는 선택 항목 처리가 필요 없는 화면이므로 OnSelect()는 오버라이드하지 않음
    // OnCancle()은 UI_Linked의 기본 닫기 로직(CloseSelf)을 그대로 사용
    public override void OnCancle()
    {
	    base.OnCancle();
	    Debug.Log("ui_bag 캔슬 호출");
    }
}
