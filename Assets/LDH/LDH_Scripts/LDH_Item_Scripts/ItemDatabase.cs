using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase
{
	//딕셔너리로 모든 아이템 보관
	//외부 접근, 수정 불가능하게 처리
	private Dictionary<string, ItemBase> _itemDict;
	
	//외부 접근용 딕셔너리 생성(readonly)
	public IReadOnlyDictionary<string, ItemBase> ItemDict => _itemDict;
	
	
	//초기화 함수
	public void Init()
	{
		_itemDict = new Dictionary<string, ItemBase>();
		//아이템 데이터 불러오기 (경로 기준 Resource/
	}
	
	//아이템 이름을 키 값으로 하는걸 기본으로 하지만
	//조건에 따라 원하는 아이템 혹은 아이템 리스트를 조회할 수 있도록 API 구현
	

}
