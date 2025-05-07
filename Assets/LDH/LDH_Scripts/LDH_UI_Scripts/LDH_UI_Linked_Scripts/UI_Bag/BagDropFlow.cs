//아이템 버리기 흐름을 담당하는 클래스

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BagDropFlow
{
	private readonly UI_Bag _bag;
	private InventorySlot _targetSlot;
	private int _maxAmount;
	private int _currentAmount;

	public BagDropFlow(UI_Bag bag)
	{
		_bag = bag;
	}
	
	//버리기 시작시 호출 하는 함수
	public void Start(InventorySlot slot)
	{
		_targetSlot = slot;
		_maxAmount = slot.Count;
		_currentAmount = 1;
		ShowAmountSelectUI();
	}
	
	
	private void ShowAmountSelectUI()
	{
		_bag.SetDescription("몇 개 버리시겠습니까?");
		
		Manager.UI.ShowCountPopup(_maxAmount,
			onConfirm: (amount) =>
			{
				_currentAmount = amount;
				ShowConfirmPopUp();
			},
			onCancel: () => Cancel());
	}
	
	private void ShowConfirmPopUp()
	{
		//문구 바꾸기
		_bag.SetDescription($"{_targetSlot.ItemName}를(을) {_currentAmount}개 버리시겠습니까?");
		
		Manager.UI.ShowConfirmPopup(
			onYes: DropItem,
			onNo: Cancel
		);
	}
	private void DropItem()
	{
		Manager.Data.PlayerData.Inventory.RemoveItem(_targetSlot, _currentAmount);
		Debug.Log($"[DropFlow] {_currentAmount}개 버리기 실행");
		_bag.Refresh();
		ShowResultMessage();
	}


	private void ShowResultMessage()
	{
		Debug.Log("[DropFlow] 버리기 완료 메시지 출력");
		Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp").ShowMessage(new List<string> {$"{_targetSlot.ItemName}을 버렸습니다!"});
	}

	public void Cancel()
	{
		Debug.Log("[DropFlow] 버리기 취소됨");
		_bag.Refresh();
	}
}
