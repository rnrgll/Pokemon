using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public abstract class ItemBase : ScriptableObject
{
	//속성 : 이름, 설명, 카테고리(대분류), 사용 대상, 사용 환경, 판매 가능, 판매 가격, 구매 가능, 구매 가격
	//메서드 : 설명 조회, 사용 가능 여부, 사용하기
	
	string name;
	string description;
	ItemCategory category;
	ItemTarget useTarget;
	ItemUseContext useContext;   

	bool isSellable;
	int sellPrice;
	bool isPurchasable;
	int purchasePrice;

	
	//필드 아이템, 중요한 물건과 같이 대상이 플레이어거나 없는 경우는 target을 null로 전제하고 구현한다.
	public abstract bool CanUse(Pokemon target, UseContext context); 
	public abstract void Use(Pokemon target, UseContext context);
	public string GetDescription() => description;

}
