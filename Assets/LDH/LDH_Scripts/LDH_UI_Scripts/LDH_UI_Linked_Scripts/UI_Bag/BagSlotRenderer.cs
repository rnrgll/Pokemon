using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 아이템 슬롯 렌더링 및 커서 선택 상태 갱신 담당
// 슬롯 오브젝트 풀링, 설명창 출력, 스크롤 위치 조정 등 포함
public class BagSlotRenderer
{
    private readonly Transform _slotRoot;
    private readonly ScrollRect _scrollRect;
    private readonly TMP_Text _descriptionText;
    private readonly UI_ItemSlot _slotPrefab;
    private readonly UI_ItemSlot _stopSlotPrefab;
    private readonly ObjectPool<UI_ItemSlot> _slotPool;

    private UI_ItemSlot _stopSlotInstance;
    private float _slotHeight = -1f;

    //나열된 
    private List<InventorySlot> _curItemList;
    // 현재 표시된 슬롯들
    public List<UI_ItemSlot> ActiveSlots { get; private set; } = new();
    // 현재 선택된 아이템
    public ItemBase SelectedItem { get; private set; }

    public BagSlotRenderer(Transform slotRoot, ScrollRect scroll, TMP_Text descText,
                           UI_ItemSlot slotPrefab, UI_ItemSlot stopSlotPrefab)
    {
        _slotRoot = slotRoot;
        _scrollRect = scroll;
        _descriptionText = descText;
        _slotPrefab = slotPrefab;
        _stopSlotPrefab = stopSlotPrefab;

        _slotPool = new ObjectPool<UI_ItemSlot>(_slotPrefab, _slotRoot);
        _slotPool.Init(20);
    }

    // 슬롯 렌더링 메서드
    public void RenderSlots(List<InventorySlot> data)
    {
	    ResetScroll();
        // 기존 슬롯 반환
        foreach (Transform child in _slotRoot)
        {
            var slot = child.GetComponent<UI_ItemSlot>();
            if (slot != null && slot != _stopSlotInstance)
            {
                slot.Deselect();
                slot.ReturnToPool();
            }
        }

        // 새로운 슬롯 생성
        foreach (var item in data)
        {
            var slot = _slotPool.Get();
            slot.SetData(item);
            //Debug.Log(slot.GetComponent<RectTransform>().rect.height);
            slot.transform.SetAsLastSibling();
        }

        // "그만두다" 슬롯 추가
        if (_stopSlotInstance == null)
        {
            _stopSlotInstance = UnityEngine.Object.Instantiate(_stopSlotPrefab, _slotRoot);
        }
        else
        {
            _stopSlotInstance.transform.SetParent(_slotRoot, false);
            _stopSlotInstance.gameObject.SetActive(true);
        }

        _stopSlotInstance.transform.SetAsLastSibling();
        ActiveSlots = GetActiveSlots();
        ResetAllSlotStates();
        
        
    }
    
    
    // 커서 이동 시 선택 상태 변경, 설명 갱신, 스크롤 이동까지 처리
    public void UpdateCursor(int preIdx, int curIdx, List<InventorySlot> data)
    {
        if (ActiveSlots.Count == 0) return;

        if (preIdx < ActiveSlots.Count) ActiveSlots[preIdx].Deselect();
        if (curIdx < ActiveSlots.Count) ActiveSlots[curIdx].Select();

        UpdateDescription(curIdx, data);
        int direction;
        if (curIdx == preIdx)
        {
	        direction = 0;
        }
        else
        {
	        direction = (int)Mathf.Sign(curIdx - preIdx);
        }
        ScrollWithinBoundary(curIdx,direction);
    }

    //설명 갱신
    private void UpdateDescription(int curIdx, List<InventorySlot> data)
    {
        string description;
        if (curIdx == ActiveSlots.Count - 1)
        {
            description = string.Empty;
            SelectedItem = null;
        }
        else
        {
            string itemName = data[curIdx].ItemName;
            SelectedItem = Manager.Data.ItemDatabase.GetItemData(itemName);
            description = SelectedItem.Description;
        }

        _descriptionText.text = description;
    }

    private void ScrollWithinBoundary(int index, int direction)
    {
	    if (_slotHeight < 0f)
	    {
		    _slotHeight = _slotPrefab.GetComponent<RectTransform>().rect.height;
	    }

	    RectTransform contentRT = _scrollRect.content;
	    RectTransform viewportRT = _scrollRect.viewport;
	    RectTransform slotRT = ActiveSlots[index].GetComponent<RectTransform>();

	    Vector3[] slotCorners = new Vector3[4];
	    slotRT.GetWorldCorners(slotCorners);

	    Vector3[] viewportCorners = new Vector3[4];
	    viewportRT.GetWorldCorners(viewportCorners);

	    float slotTop = slotCorners[1].y;
	    float slotBottom = slotCorners[0].y;
	    float viewTop = viewportCorners[1].y;
	    float viewBottom = viewportCorners[0].y;

	    bool isAbove = slotTop > viewTop;
	    bool isBelow = slotBottom < viewBottom;

	    
	    //Debug.Log(slotRT.transform.GetChild(2).GetComponent<TMP_Text>().text + " : " + isAbove.ToString() + " , " + isBelow);
	    if (isAbove || isBelow)
	    {
		    Vector2 pos = contentRT.anchoredPosition;
		    pos.y += direction * _slotHeight;

		    float contentHeight = contentRT.rect.height;
		    float viewHeight = viewportRT.rect.height;
		    float maxScrollY = Mathf.Max(0, contentHeight - viewHeight);

		    pos.y = Mathf.Clamp(pos.y, 0, maxScrollY);
		    contentRT.anchoredPosition = pos;
	    }
    }
    
    
    private List<UI_ItemSlot> GetActiveSlots()
    {
        List<UI_ItemSlot> slots = new();
        for (int i = 0; i < _slotRoot.childCount; i++)
        {
            Transform child = _slotRoot.GetChild(i);
            if (child.gameObject.activeSelf)
                slots.Add(child.GetComponent<UI_ItemSlot>());
        }

        return slots;
    }
    
    
    public void ResetAllSlotStates()
    {
	    for (int i = 0; i < ActiveSlots.Count; i++)
	    {
		    // if (i == activeIdx)
			   //  ActiveSlots[i].Select(); // 꽉찬 화살표 + 활성화
		    // else
			    ActiveSlots[i].Deselect(); // 빈 화살표 + 비활성화
			    ActiveSlots[i].ChangeArrow(true);
	    }
    }

    public void ResetScroll()
    {
	    if (_scrollRect != null && _scrollRect.content != null)
	    {
		    Vector2 pos = _scrollRect.content.anchoredPosition;
		    pos.y = 0f;
		    _scrollRect.content.anchoredPosition = pos;
		    _scrollRect.verticalNormalizedPosition = 1f;

	    }
    }

}
