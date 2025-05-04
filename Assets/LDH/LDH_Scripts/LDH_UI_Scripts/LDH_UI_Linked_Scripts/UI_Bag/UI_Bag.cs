using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
	private float slotHeight = -1f;

	private List<InventorySlot> curItemList;
	private List<UI_ItemSlot> activeItemSlots = new(); // 현재 UI에서 활성화된 슬롯만 추려서 저장

	private ItemBase selectItem;
	#endregion
	
	#region UI 요소 참조 및 리소스

	[Header("UI 오브젝트 참조")]
	[SerializeField] private Image bagIconImg;
	[SerializeField] private Image labelImg;
	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private TMP_Text descriptionText;
	
	private Transform itemSlotRoot;

	[Header("리소스")]
	[SerializeField] private Sprite[] bagIconSprites;
	[SerializeField] private Sprite[] labelSprites;
	[SerializeField] private UI_ItemSlot uiItemSlotPrefab;
	[SerializeField] private UI_ItemSlot stopSlotPrefab;
	
	private ObjectPool<UI_ItemSlot> slotPool;
	private UI_ItemSlot stopSlotInstance; // "그만두다" 슬롯 인스턴스


	#endregion
	
	//======== 코드 분리 중에 생성한 변수들 =======//
	private BagInputHandler _inputHandler; //입력 처리 로직 분리, 핸들러에게 위임
	private BagPopupManager _popupManager; //팝업 생성 매니져
	
	#region Unity 라이프사이클

	private void Awake()
	{
		itemSlotRoot = scrollRect.content;
		uiItemSlotPrefab = Resources.Load<UI_ItemSlot>("UI_Prefabs/Component/UI_ItemSlot");
		stopSlotPrefab = Resources.Load<UI_ItemSlot>("UI_Prefabs/Component/UI_ItemSlot_Quit");

		slotPool = new ObjectPool<UI_ItemSlot>(uiItemSlotPrefab, itemSlotRoot);
		slotPool.Init(20);
		
		//초기화 필요한 것들 초기화~
		_inputHandler = new BagInputHandler(this);
		_popupManager = new BagPopupManager(this);
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

	public override void HandleInput(UIInputType inputType)
	{
		_inputHandler.Handle(inputType); //위임
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
			slot.transform.SetAsLastSibling(); // 정렬보장!!!
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
		stopSlotInstance.transform.SetAsLastSibling();
	}

	/// <summary>
	/// 현재 커서 위치 UI 반영
	/// </summary>
	private void UpdateCursor()
	{
		if (activeItemSlots.Count == 0) return;
		
		//Debug.Log($"커서 업데이트 : {preCursorIdx} => {curCursorIdx}");

		if (preCursorIdx < activeItemSlots.Count)
			activeItemSlots[preCursorIdx].Deselect();
		if (curCursorIdx < activeItemSlots.Count)
			activeItemSlots[curCursorIdx].Select();
		
		UpdateDescription();
	}

	//여기서 아이템 정보를 캐싱한다.
	private void UpdateDescription()
	{
		string description;
		if (curCursorIdx == activeItemSlots.Count - 1)
		{
			//그만두다의 경우 설명 공란으로 표시
			description = String.Empty;
			
			//선택된 아이템 null로 초기화
			selectItem = null;
		}
		else
		{
			string itemName = curItemList[curCursorIdx].ItemName;
			selectItem = Manager.Data.ItemDatabase.GetItemData(itemName);
			description = selectItem.Description;
		}
		descriptionText.text = description;
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
	
	
	//스크롤 갱신
	private void ScrollToIndexBySlotHeight(int index)
	{
		if (slotHeight < 0f)
		{
			slotHeight = uiItemSlotPrefab.GetComponent<RectTransform>().rect.height;
		}
		float offset = index * slotHeight;

		Vector2 pos = scrollRect.content.anchoredPosition;
		scrollRect.content.anchoredPosition = new Vector2(pos.x, offset);
		
	}

	#endregion
	
	
	#region 커서 및 탭 이동 관련

	public void MovePanel(int direction)
	{
		int max = (int)ItemCategory.Count;
		int next = ((int)currentCategory + direction + max) % max;

		currentCategory = (ItemCategory)next;

		Refresh(); // 카테고리 변경 시 전체 갱신
	}

	public void MoveCursor(int direction)
	{
		int panelIdx = (int)currentCategory;

		preCursorIdx = curCursorIdx;
		
		int max = activeItemSlots.Count - 1;
		int next = Mathf.Clamp(curCursorIdx + direction, 0, max);
		
		curCursorIdx = next;

		currentCursorList[panelIdx] = curCursorIdx;

		UpdateCursor();
		//UpdateScrollBySLotHeight(curCursorIdx, activeItemSlots.Count);
		ScrollToIndexBySlotHeight(curCursorIdx); 

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

	public override void OnSelect()
	{
		//그만두다를 선택한 경우
		if (curCursorIdx == activeItemSlots.Count - 1)
		{
			CloseSelf();
			return;
		}
		
		//화살표 바꾸기 (빈 빨강 화살표로)
		activeItemSlots[curCursorIdx].ChangeArrow(false);
		
		//조건에 따라 팝업 생성하는건 팝업 매니저가 처리하기
		_popupManager.ShowItemActionPopup(selectItem); //위임
	}

	public override void OnCancle()
	{
		base.OnCancle();
		Debug.Log("UI_Bag: 닫힘 처리됨");
	}

	#endregion



	void SetPopUpPosition(UI_SelectPopUp selectPopUp)
	{
		// Box의 RectTransform
		RectTransform boxRT = selectPopUp.transform.GetChild(0).GetComponent<RectTransform>();

		// 1. 앵커: 왼쪽 하단 고정
			boxRT.anchorMin = new Vector2(0f, 0f);
			boxRT.anchorMax = new Vector2(0f, 0f);

		// 2. 피벗: 왼쪽 하단
			boxRT.pivot = new Vector2(0f, 0f);

		// 3. 캔버스 높이 기준 위치 지정
			float canvasHeight = ((RectTransform)selectPopUp.transform).rect.height;
			float y = canvasHeight * 0.34f;

		boxRT.anchoredPosition = new Vector2(0f, y);
	}
}
