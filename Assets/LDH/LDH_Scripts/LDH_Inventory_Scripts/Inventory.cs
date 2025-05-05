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
		var item = Manager.Data.ItemDatabase.GetItemData(itemName);
		
		if(item.Use(null,InGameContextFactory.CreateBasic(isBattle: false))) //수정하기
		{
			RemoveItem(itemName);
		}
	}
	
	/// <summary>
	/// 아이템 이름을 기준으로 인벤토리에서 지정한 수량만큼 제거
	/// 수량이 0 이하가 되면 슬롯 자체를 삭제
	/// </summary>
	/// <param name="itemName">제거할 아이템의 이름</param>
	/// <param name="amount">제거할 수량 (기본값: 1)</param>
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
	
	/// <summary>
	/// 인벤토리 슬롯을 직접 지정하여 아이템을 제거
	/// 수량이 0 이하가 되면 슬롯 자체를 삭제
	/// </summary>
	/// <param name="slot">제거 대상이 되는 인벤토리 슬롯</param>
	/// <param name="amount">제거할 수량 (기본값: 1)</param>
	public void RemoveItem(InventorySlot slot, int amount = 1)
	{
		if (slot == null || !_slots.Contains(slot)) return;

		slot.Count -= amount;
		if (slot.Count <= 0)
		{
			_slots.Remove(slot);
			_slotLookUp.Remove(slot.ItemName);
		}
	}


	public bool CanUse(string itemName)
	{
		var item = Manager.Data.ItemDatabase.GetItemData(itemName);
		return item.CanUseNow(InGameContextFactory.CreateBasic(isBattle: false)); //todo: 배틀중인지 아닌지 관리하는 변수 넣어줘야함
	} 

	

	public List<InventorySlot> GetItemsByCategory(Define.ItemCategory itemCategory)
	{
		if (itemCategory != Define.ItemCategory.TM_HM)
		{
			return _slots
				.Where(slot => Manager.Data.ItemDatabase.CheckItemCategory(slot.ItemName, itemCategory))
				.ToList();
		}
		
		//기술머신 정렬
		List<InventorySlot> tmList = new();
		List<InventorySlot> hmList = new();
		foreach (var slot in _slots)
		{
			var data = Manager.Data.ItemDatabase.GetItemData(slot.ItemName) as Item_SkillMachine;
			if (data == null) continue;
		
			if (data.MachineType == SkillMachineType.TM)
				tmList.Add(slot);
			else if (data.MachineType == SkillMachineType.HM)
				hmList.Add(slot);
		}
		
		//각각 정렬
		tmList.Sort(CompareByMachineNumber);
		hmList.Sort(CompareByMachineNumber);
		// 이어 붙이기
		tmList.AddRange(hmList);
		return tmList;
		
	}
	
	//기술 번호 기준 오름차순 정렬
	private int CompareByMachineNumber(InventorySlot a, InventorySlot b)
	{
		var aData = Manager.Data.ItemDatabase.GetItemData(a.ItemName) as Item_SkillMachine;
		var bData = Manager.Data.ItemDatabase.GetItemData(b.ItemName) as Item_SkillMachine;

		if (aData == null || bData == null) return 0; // 예외 방지

		return aData.MachineNumber.CompareTo(bData.MachineNumber);
	}


	
}