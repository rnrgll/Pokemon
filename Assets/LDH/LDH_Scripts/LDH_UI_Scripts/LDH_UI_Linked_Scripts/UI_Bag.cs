using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static Define;
public class UI_Bag : UI_Linked
{
	
	#region 인벤토리 & 커서 상태 관리

	private static ItemCategory currentCategory = ItemCategory.Item;
	private static int[] currentCursorList = new int[(int)ItemCategory.Count];
	private int curCursorIdx = 0;
	private int preCursorIdx = 0;

	private List<InventorySlot> curItemList;
	private List<UI_ItemSlot> activeItemSlots = new(); // 현재 UI에서 활성화된 슬롯만 추려서 저장
	#endregion
	
	#region UI 요소 참조 및 리소스

	[Header("UI 오브젝트 참조")]
	[SerializeField] private Image bagIconImg;
	[SerializeField] private Image labelImg;
	[SerializeField] private ScrollRect scrollRect;
	private Transform itemSlotRoot;

	[Header("리소스")]
	[SerializeField] private Sprite[] bagIconSprites;
	[SerializeField] private Sprite[] labelSprites;
	[SerializeField] private UI_ItemSlot uiItemSlotPrefab;
	[SerializeField] private UI_ItemSlot stopSlotPrefab;
	
	private ObjectPool<UI_ItemSlot> slotPool;
	private UI_ItemSlot stopSlotInstance; // "그만두다" 슬롯 인스턴스


	#endregion
	
	
	#region Unity 라이프사이클

	private void Awake()
	{
		itemSlotRoot = scrollRect.content;
		uiItemSlotPrefab = Resources.Load<UI_ItemSlot>("UI_Prefabs/Component/UI_ItemSlot");
		stopSlotPrefab = Resources.Load<UI_ItemSlot>("UI_Prefabs/Component/UI_ItemSlot_Quit");

		slotPool = new ObjectPool<UI_ItemSlot>(uiItemSlotPrefab, itemSlotRoot);
		slotPool.Init(20);
	}

	private void OnEnable()
	{
		Refresh(); // 진입 시 전체 UI 초기화
	}

	protected override void Init()
	{
		base.Init();
		Refresh();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow)) MovePanel(-1);
		else if (Input.GetKeyDown(KeyCode.RightArrow)) MovePanel(1);
		else if (Input.GetKeyDown(KeyCode.UpArrow)) MoveCursor(-1);
		else if (Input.GetKeyDown(KeyCode.DownArrow)) MoveCursor(1);
	}

	#endregion

	#region 전체 갱신(데이터 갱신 + UI 갱신)

	/// <summary>
	/// 전체 갱신 루틴: 인벤토리 데이터 조회 + UI 갱신
	/// </summary>
	private void Refresh()
	{
		RefreshItemData();
		RefreshUI();
	}

	#endregion

	#region 인벤토리 데이터 갱신

	/// <summary>
	/// 현재 탭에 해당하는 카테고리의 아이템 리스트를 인벤토리에서 가져와 갱신
	/// </summary>
	private void RefreshItemData()
	{
		curItemList = Manager.Data.PlayerData.Inventory.GetItemsByCategory(currentCategory);

		int panelIdx = (int)currentCategory;
		curCursorIdx = currentCursorList[panelIdx];
		preCursorIdx = curCursorIdx;
	}
	
	#endregion

	#region UI 갱신

	/// <summary>
	/// 전체 UI 구성 갱신 (아이콘, 태그, 슬롯, 커서)
	/// </summary>
	private void RefreshUI()
	{
		UpdateBagIcon(currentCategory);
		UpdateLabelImage(currentCategory);
		UpdateItemSlots();
		
		activeItemSlots = GetActiveItemSlots(); // 커서 이동 & 선택 공용
		UpdateCursor();
	}

	/// <summary>
	/// 아이템 슬롯 UI 갱신 (오브젝트 풀 사용)
	/// </summary>
	private void UpdateItemSlots()
	{
		// 기존 슬롯 전부 반납
		foreach (Transform child in itemSlotRoot)
		{
			var slot = child.GetComponent<UI_ItemSlot>();
			if (slot != null)
			{
				slot.Deselect();
				if(slot != stopSlotInstance)
					slot.ReturnToPool();
			}
			
			
		}

		// 새로운 슬롯 생성 및 데이터 반영
		foreach (InventorySlot item in curItemList)
		{
			UI_ItemSlot slot = slotPool.Get();
			slot.SetData(item);
		}
		
		// "그만두다" 슬롯 추가
		if (stopSlotInstance == null)
		{
			stopSlotInstance = Instantiate(stopSlotPrefab, itemSlotRoot);
		}
		else
		{
			stopSlotInstance.transform.SetParent(itemSlotRoot, false);
			stopSlotInstance.gameObject.SetActive(true);
		}
	}

	/// <summary>
	/// 현재 커서 위치 UI 반영
	/// </summary>
	private void UpdateCursor()
	{
		if (activeItemSlots.Count == 0) return;
		
		Debug.Log($"커서 업데이트 : {preCursorIdx} => {curCursorIdx}");

		if (preCursorIdx < activeItemSlots.Count)
			activeItemSlots[preCursorIdx].Deselect();
		if (curCursorIdx < activeItemSlots.Count)
			activeItemSlots[curCursorIdx].Select();

	}

	//가방 이미지 갱신
	private void UpdateBagIcon(ItemCategory category)
	{
		bagIconImg.sprite = bagIconSprites[(int)category];
	}
	//라벨 이미지 갱신
	private void UpdateLabelImage(ItemCategory category)
	{
		labelImg.sprite = labelSprites[(int)category];
	}

	#endregion
	
	
	#region 커서 및 탭 이동 관련

	private void MovePanel(int direction)
	{
		int max = (int)ItemCategory.Count;
		int next = ((int)currentCategory + direction + max) % max;

		currentCategory = (ItemCategory)next;

		Refresh(); // 카테고리 변경 시 전체 갱신
	}

	private void MoveCursor(int direction)
	{
		int panelIdx = (int)currentCategory;

		preCursorIdx = curCursorIdx;
		
		int max = activeItemSlots.Count - 1;
		int next = Mathf.Clamp(curCursorIdx + direction, 0, max);
		
		curCursorIdx = next;

		currentCursorList[panelIdx] = curCursorIdx;

		UpdateCursor();
	}

	private List<UI_ItemSlot> GetActiveItemSlots()
	{
		List<UI_ItemSlot> activeSlots = new();
		for (int i = 0; i < itemSlotRoot.childCount; i++)
		{
			Transform slot = itemSlotRoot.GetChild(i);
			if(slot.gameObject.activeSelf)
				activeSlots.Add(slot.GetComponent<UI_ItemSlot>());
		}

		return activeSlots;
	}

	#endregion

	#region UI_Linked 오버라이드
	//해당 UI는 선택 항목 처리가 필요 없는 화면이므로 OnSelect()는 오버라이드하지 않음
	// OnCancle()은 UI_Linked의 기본 닫기 로직(CloseSelf)을 그대로 사용
	public override void OnCancle()
	{
		base.OnCancle();
		Debug.Log("UI_Bag: 닫힘 처리됨");
	}

	#endregion
}
