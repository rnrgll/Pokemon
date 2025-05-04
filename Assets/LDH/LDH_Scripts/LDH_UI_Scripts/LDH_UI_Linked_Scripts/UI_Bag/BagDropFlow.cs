//아이템 버리기 흐름을 담당하는 클래스

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
		// TODO: UI_DropAmountPopup 만들고 이곳에서 Show
		
		_bag.SetDescription("몇 개 버리시겠습니까?");
		
		_bag.PopupManager.ShowCountPopup(_maxAmount,
			onConfirm: (amount) =>
			{
				_currentAmount = amount;
				ShowConfirmPopup();
			},
			onCancel: () => Cancel());
	}

	private void ShowConfirmPopup()
	{
		// TODO: 확인 창 생성
		Debug.Log($"[DropFlow] {_currentAmount}개를 버릴까요? 예/아니오");
	}

	private void DropItem()
	{
		// TODO: 인벤토리에서 실제 아이템 제거
		Debug.Log($"[DropFlow] {_currentAmount}개 버리기 실행");
	}

	private void ShowResultMessage()
	{
		// TODO: "버렸습니다" 메시지 출력
		Debug.Log("[DropFlow] 버리기 완료 메시지 출력");
		_bag.Refresh(); // UI 갱신
	}

	public void Cancel()
	{
		Debug.Log("[DropFlow] 버리기 취소됨");
	}
}
