using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static Define;
public class UI_Bag : UI_Linked
{
	private static ItemCategory currentPanel = ItemCategory.Item;
	private static int[] currentCursorList = new int[(int)ItemCategory.Count];
	private int curCursorIdx = 0;
	private int preCursorIdx = 0;
	
	[Header("UI 오브젝트 참조")]
	[SerializeField] private Image bagIconImg;
	[SerializeField] private Image labelImg;
	[SerializeField] private ScrollRect scrollRect;
	private Transform itemSlotRoot;

	[Header("리소스")] 
	[SerializeField] private Sprite[] bagIconSprites;
	[SerializeField] private Sprite[] labelSprites;

	private void Awake()
	{
		itemSlotRoot = scrollRect.content;
	}

	private void OnEnable()
	{
		UpdateUI();
	}

	protected override void Init()
	{
		base.Init();
		UpdateUI();
		UpdateItemSlotUI();
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
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			MoveCursor(-1);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			MoveCursor(1);
		}
	}
	
	
	void MovePanel(int direction)
	{
		int next = (int)currentPanel + direction;
		int max = (int)Define.ItemCategory.Count;

		if (next < 0) next = max - 1;
		else if (next >= max) next = 0;

		currentPanel = (Define.ItemCategory)next;

		UpdateUI();
	}

	void MoveCursor(int direction)
	{
		int panelIdx = (int)currentPanel;
		
		preCursorIdx = curCursorIdx;
		int next = curCursorIdx + direction;
		int itemCount = itemSlotRoot.childCount;
		next = Mathf.Clamp(next, 0, itemCount-1);
		
		Debug.Log($"{itemCount} , {next}");
		currentCursorList[panelIdx] = curCursorIdx = next;
		UpdateItemSlotUI();
	}

	void UpdateUI()
	{
		UpdateBagIcon(currentPanel);
		UpdateTagImage(currentPanel);
	}

	void UpdateItemSlotUI()
	{
		UI_ItemSlot preSlot = itemSlotRoot.GetChild(preCursorIdx).GetComponent<UI_ItemSlot>();
		UI_ItemSlot curSlot = itemSlotRoot.GetChild(curCursorIdx).GetComponent<UI_ItemSlot>();
		
		preSlot.Deselect();
		curSlot.Select();
	}



	private void UpdateBagIcon(Define.ItemCategory panel)
	{
		bagIconImg.sprite = bagIconSprites[(int)panel];
	}

	private void UpdateTagImage(Define.ItemCategory panel)
	{
		labelImg.sprite = labelSprites[(int)panel];
	}
	//private void UpdateItemList(Define.ItemCategory panelType) { /* ... */ }


	
	
	
	
	
	
    //해당 UI는 선택 항목 처리가 필요 없는 화면이므로 OnSelect()는 오버라이드하지 않음
    // OnCancle()은 UI_Linked의 기본 닫기 로직(CloseSelf)을 그대로 사용
    public override void OnCancle()
    {
	    base.OnCancle();
	    Debug.Log("ui_bag 캔슬 호출");
    }
}

