using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public abstract class ItemBase : ScriptableObject
{
	//속성 : 이름, 설명, 카테고리(대분류), 사용 대상, 사용 환경, 판매 가능, 판매 가격, 구매 가능, 구매 가격
	//메서드 : 설명 조회, 사용 가능 여부, 사용하기
	
	[SerializeField] string name;
	[SerializeField] string description;
	[SerializeField] ItemCategory category;
	[SerializeField] ItemTarget useTarget;
	[SerializeField] ItemUseContext useContext;   

	[SerializeField] bool isSellable;
	[SerializeField] int sellPrice;
	[SerializeField] bool isPurchasable;
	[SerializeField] int purchasePrice;

	[SerializeField] private bool isConsumable;
	
	public string Name => name;
	public string Description => description;
	public ItemCategory Category => category; //카테고리
	public ItemTarget UseTartget => useTarget; //사용 대상
	public ItemUseContext UseContext => useContext; //사용 환경

	
	public bool IsSellable => isSellable;
	public bool IsPurchasable => isPurchasable;
	public int SellPrice => sellPrice;
	public int PurchasePrice => purchasePrice;
	public bool IsConsumable => isConsumable;
	
	
	/// <summary>
	/// 이 아이템이 대상 선택이 필요한지 여부
	/// 기본값: 대상이 포켓몬이면 true
	/// 필요하다면 오버라이딩 가능
	/// </summary>
	public virtual bool RequiresTarget()
	{
		return (useTarget == ItemTarget.EmneyPokemon || useTarget == ItemTarget.MyPokemon);
	}
	
	/// <summary>
	/// 현재 상황(context)에서 사용 가능한 아이템인가?
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public virtual bool CanUseNow(InGameContext curInGameContext)
	{
		switch (useContext)
		{
			case ItemUseContext.Both:
				return true;
			case ItemUseContext.BattleOnly:
				return curInGameContext.IsInBattle;
			case ItemUseContext.FieldOnly:
				return !curInGameContext.IsInBattle;
			default:
				return false;
		}
	}
	public abstract bool Use(Pokémon target, InGameContext inGameContext);
}
