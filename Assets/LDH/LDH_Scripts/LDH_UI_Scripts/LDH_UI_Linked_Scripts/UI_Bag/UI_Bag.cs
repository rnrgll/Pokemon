using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static Define;

// 인벤토리 UI 전체 흐름을 담당하는 클래스
// 책임 분산: 입력 처리, 팝업 생성, 슬롯 렌더링은 별도 클래스로 위임
public class UI_Bag : UI_Linked
{
	
	#region  현재 탭(카테고리) 정보와 커서 위치 관리

	private static ItemCategory currentCategory = ItemCategory.Item;
	public ItemCategory CurrentCategory => currentCategory;
	private static int[] currentCursorList = new int[(int)ItemCategory.Count];
	private int curCursorIdx = 0;
	private int preCursorIdx = 0;

	private List<InventorySlot> curItemList;

	#endregion
	
	#region UI 참조 및 리소스 (가방 아이콘, 라벨, 스크롤 영역 등)

	[Header("UI 오브젝트 참조")]
	[SerializeField] private Image bagIconImg;
	[SerializeField] private Image labelImg;
	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private TMP_Text descriptionText;
	
	private Transform itemSlotRoot;

	[Header("리소스")]
	[SerializeField] private Sprite[] bagIconSprites;
	[SerializeField] private Sprite[] labelSprites;
	private UI_ItemSlot uiItemSlotPrefab;
	private UI_ItemSlot stopSlotPrefab;


	#endregion

	#region 분리된 클래스들

	private BagInputHandler _inputHandler; // 입력 처리 담당 (↑↓←→ 선택 취소 등)
	private BagPopupManager _popupManager; // 아이템 선택 시 팝업 생성 및 구성 담당
	public BagPopupManager PopupManager => _popupManager; // 아이템 선택 시 팝업 생성 및 구성 담당
	private BagSlotRenderer _slotRenderer; // 슬롯 UI 갱신 및 설명창 출력 담당
	#endregion

	#region 인벤토리 기능 관련 클래스들

	private BagDropFlow _bagDropFlow;
	private BagUseFlow _bagUseFlow;

	#endregion
	
	
	#region Unity 라이프사이클

	private void Awake()
	{
		itemSlotRoot = scrollRect.content;
		uiItemSlotPrefab = Resources.Load<UI_ItemSlot>("UI_Prefabs/Component/UI_ItemSlot");
		stopSlotPrefab = Resources.Load<UI_ItemSlot>("UI_Prefabs/Component/UI_ItemSlot_Quit");
		
		//초기화 필요한 것들 초기화~
		_inputHandler = new BagInputHandler(this);
		_popupManager = new BagPopupManager(this);
		_slotRenderer = new BagSlotRenderer(
			itemSlotRoot, scrollRect, descriptionText,
			Resources.Load<UI_ItemSlot>("UI_Prefabs/Component/UI_ItemSlot"),
			Resources.Load<UI_ItemSlot>("UI_Prefabs/Component/UI_ItemSlot_Quit")
		);

		_bagDropFlow = new BagDropFlow(this);
		_bagUseFlow = new BagUseFlow(this);

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
	public void Refresh()
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
		_slotRenderer.RenderSlots(curItemList);
		_slotRenderer.UpdateCursor(preCursorIdx, curCursorIdx, curItemList);

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

	public void MovePanel(int direction)
	{
		int max = (int)ItemCategory.Count;
		int next = ((int)currentCategory + direction + max) % max;

		currentCategory = (ItemCategory)next;

		//패널이동시에만 인덱스를 초기화시킨다.
		preCursorIdx = curCursorIdx = 0;
		currentCursorList[(int)currentCategory] = curCursorIdx;
		
		Refresh(); // 카테고리 변경 시 전체 갱신
		
	}

	public void MoveCursor(int direction)
	{
		int panelIdx = (int)currentCategory;

		preCursorIdx = curCursorIdx;
		int max = _slotRenderer.ActiveSlots.Count - 1;
		int next = Mathf.Clamp(curCursorIdx + direction, 0, max);
		curCursorIdx = next;

		currentCursorList[panelIdx] = curCursorIdx;

		_slotRenderer.UpdateCursor(preCursorIdx, curCursorIdx, curItemList);
	}

	#endregion
	
	
	#region UI_Linked 오버라이드

	public override void OnSelect()
	{
		//그만두다를 선택한 경우
		if (curCursorIdx == (_slotRenderer.ActiveSlots.Count-1))
		{
			CloseSelf();
			return;
		}
		
		//화살표 바꾸기 (빈 빨강 화살표로)
		_slotRenderer.ActiveSlots[curCursorIdx].ChangeArrow(false);
		
		//조건에 따라 팝업 생성하는건 팝업 매니저가 처리하기 - 선택한 아이템 슬롯 정보 넘기기
		_popupManager.ShowItemActionPopup(curItemList[curCursorIdx]); //위임
	}

	public override void OnCancel()
	{
		base.OnCancel();
		Debug.Log("UI_Bag: 닫힘 처리됨");
	}

	#endregion


	public void SetDescription(string text)
	{
		descriptionText.text = text;
	}
	
	
	//인벤토리 기능 호출용
	//버리기 기능 - 버릴 아이템의 슬롯 정보(인벤토리 슬롯 정보)을 넘겨준다.
	public void StartDropFlow(InventorySlot slot)
	{
		_bagDropFlow.Start(slot);
		
	}
	
	//사용하기 기능
	public void StartUseFlow(InventorySlot slot)
	{
		_bagUseFlow.Start(slot);
	}
	
	

}
