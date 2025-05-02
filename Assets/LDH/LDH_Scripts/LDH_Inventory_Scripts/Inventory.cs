using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
	//인벤토리
	private List<InventorySlot> _slots;
	private Dictionary<string, InventorySlot> _slotLookUp; //빠른 검색을 위한 보조 인덱스

	public ItemBase selectItem;
	private InventorySlot selectSlot;
	
	
	public IReadOnlyList<InventorySlot> Slots => _slots;

	public void Init()
	{
		_slots = new();
		_slotLookUp = new();
		selectItem = null;
	}
	
	//디버깅용
	public void MakeTempInventory(List<InventorySlot> slots)
	{
		foreach (InventorySlot sl in slots)
		{
			AddItem(sl.ItemName, sl.Count);
		}
	}

	public void AddItem(string itemName, int amount = 1)
	{
		if (_slotLookUp.TryGetValue(itemName, out InventorySlot slot))
		{
			//인벤토리에 있는 아이템인 경우
			slot.Count += amount;
		}
		//새로운 아이템 추가인 경우
		else
		{
			InventorySlot newSlot = new InventorySlot { ItemName = itemName, Count = amount };
			_slots.Add(newSlot);
			_slotLookUp[itemName] = newSlot;
		}
	}

	public void UseItem(string itemName)
	{
		//todo:ui 연동
		//상황에 따라 분기
		
		//아이템 사용에 성공하면 1개 감소 처리
		//실패하면 아무것도 하지 않는다.
		if (selectItem.ItemName != itemName)
		{
			 selectItem = Manager.Data.ItemDatabase.GetItemData(itemName);
		}
		
		if(selectItem.Use(null,InGameContextFactory.CreateBasic(isBattle: false))) //수정하기
		{
			RemoveItem(itemName);
		}
	}
	
	public void RemoveItem(string itemName, int amount = 1)
	{
		if (!_slotLookUp.TryGetValue(itemName, out var slot)) return;

		slot.Count -= amount;
		if (slot.Count <= 0)
		{
			_slots.Remove(slot);
			_slotLookUp.Remove(itemName);
		}
	}
	

	public bool CanUse(string itemName)
	{
		selectItem = Manager.Data.ItemDatabase.GetItemData(itemName);
		return selectItem.CanUseNow(InGameContextFactory.CreateBasic(isBattle: false)); //todo: 배틀중인지 아닌지 관리하는 변수 넣어줘야함

	}

	public List<InventorySlot> GetItemsByCategory(Define.ItemCategory itemCategory)
	{
		return _slots.Where(slot =>
			Manager.Data.ItemDatabase.CheckItemCategory(slot.ItemName, itemCategory)).ToList();
	}
	
}