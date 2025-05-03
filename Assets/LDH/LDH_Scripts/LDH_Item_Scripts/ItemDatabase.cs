using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ItemDatabase
{
	//딕셔너리로 모든 아이템 보관
	//외부 접근, 수정 불가능하게 처리
	[SerializeField] private Dictionary<string, ItemBase> _itemDict;
	//외부 접근용 딕셔너리 생성(readonly)
	public IReadOnlyDictionary<string, ItemBase> ItemDict => _itemDict;
	
	
	//초기화 함수
	public void Init()
	{
		_itemDict = new Dictionary<string, ItemBase>();
		//아이템 데이터 불러오기 (경로 Resource/Item_Data/)
		var items = Resources.LoadAll<ItemBase>("Item_Data");


		//이름 중복 체크하며 딕셔너리에 저장
		foreach (ItemBase item in items)
		{
			
			string key = item.ItemName;
			
			if (!_itemDict.ContainsKey(key))
			{
				_itemDict.Add(key, item);
			}
			else
			{
				Debug.LogWarning($"중복된 아이템 : {key}");
			}
			
			
		}
		Debug.Log($"ItemDatabase - 아이템 로드 완료. 총 아이템 수 {_itemDict.Count}");
	}
	
	//아이템 이름을 키 값으로 하는걸 기본으로 하지만
	//조건에 따라 원하는 아이템 혹은 아이템 리스트를 조회할 수 있도록 API 구현
	
	//1. 아이템 이름으로 아이템을 검색하여 반환
	public ItemBase GetItemData(string itemName)
	{
		if (_itemDict.TryGetValue(itemName, out ItemBase item))
		{
			return item;
		}
		Debug.Log($"{itemName} 아이템을 찾을 수 없습니다.");
		return null;

	}
	
	//2. 카테고리로 아이템 필더
	public List<ItemBase> GetItemsByCategory(Define.ItemCategory category)
	{
		return _itemDict.Values
			.Where(item => item.Category == category)
			.ToList();
	}
	
	//3. 아이템이 해당 카테고리인지 여부 반환
	/// <summary>
	/// 아이템이 해당 카테고리인지 여부 반환
	/// </summary>
	/// <param name="itemName"></param>
	/// <param name="category"></param>
	/// <returns></returns>
	public bool CheckItemCategory(string itemName, Define.ItemCategory category)
	{
		return GetItemData(itemName)?.Category == category;
	}
	
	//
	// //3. 판매 가능한 아이템 추출
	// public List<ItemBase> GetSellableItems()
	// {
	// 	return _itemDict.Values
	// 		.Where(item => item.IsSellable)
	// 		.ToList();
	// }
	//
	// //4. 사용 가능한 상황 필터
	// public List<ItemBase> GetUsableInContext(InGameContext context)
	// {
	// 	return _itemDict.Values
	// 		.Where(item => item.CanUseNow(context))
	// 		.ToList();
	// }
	

}
