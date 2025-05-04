using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 아이템 선택 시 나타나는 팝업(UI_SelectPopUp)을 생성하고 구성하는 책임
// 팝업 내부의 버튼 옵션 구성과 위치 조정까지 담당
public class BagPopupManager
{
	private readonly UI_Bag _bag;

	public BagPopupManager(UI_Bag bag)
	{
		_bag = bag;
	}

	public void ShowItemActionPopup(InventorySlot slot)
	{
		ItemBase item = Manager.Data.ItemDatabase.GetItemData(slot.ItemName);
		bool? canUse = item?.CanUseNow(InGameContextFactory.CreateFromGameManager());

		// 팝업 생성 및 옵션 설정
		var popup = Manager.UI.ShowPopupUI<UI_SelectPopUp>("UI_SelectablePopUp");

		if (canUse == true)
		{
			switch (item.Category)
			{
				case Define.ItemCategory.KeyItem:
					case Define.ItemCategory.TM_HM:
						popup.SetupOptions(new()
						{
							("사용하다", new CustomAction(() => _bag.StartUseFlow(slot))),
							("그만두다", new CustomAction(popup.OnCancel))
						});
						break;
				default:
					popup.SetupOptions(new()
					{
						("사용하다", new CustomAction(() => _bag.StartUseFlow(slot))),
						("버리다", new CustomAction(() =>
						{
							_bag.StartDropFlow(slot);
					
						})),
						("그만두다", new CustomAction(popup.OnCancel))
					});
					break;
			}
	
		}
		else
		{
			popup.SetupOptions(new()
			{
				("그만두다", new CustomAction(popup.OnCancel))
			});
		}

		RectTransform boxRT = popup.transform.GetChild(0).GetComponent<RectTransform>();
		// float canvasHeight = ((RectTransform)popup.transform).rect.height;
		// Util.SetPositionFromBottomLeft(boxRT, 0f, canvasHeight * 0.34f);
		Canvas canvas = boxRT.GetComponentInParent<Canvas>();
		Util.SetPositionFromBottomLeft(boxRT, 0f, 0f);
		Util.SetRelativeVerticalOffset(boxRT,canvas,0.34f);
	}

	public void ShowCountPopup(int maxAmount, Action<int> onConfirm, Action onCancel)
	{
		var countUI = Manager.UI.ShowPopupUI<UI_CountPopUp>("UI_CountPopUp");
		countUI.Init(
			maxAmount, onConfirm, onCancel
		);
		
		RectTransform boxRT = countUI.transform.GetChild(0).GetComponent<RectTransform>();
		Canvas canvas = boxRT.GetComponentInParent<Canvas>();
		
		Util.SetPositionFromBottomRight(boxRT, 0f, 0f);
		Util.SetRelativeVerticalOffset(boxRT,canvas,0.34f);
	}
	
	public void ShowConfirmPopup(Action onYes, Action onNo = null)
	{
		//아니오 액션 저장
		ISelectableAction noAction = new CustomAction(() => onNo?.Invoke());
		
		var popup = Manager.UI.ShowPopupUI<UI_SelectPopUp>("UI_SelectablePopUp");
		
		

		popup.SetupOptions(new()
		{
			("예", new CustomAction(() => {
				onYes?.Invoke();
			})),
			("아니오", noAction)
		});
		
		popup.OverrideCancelAction(noAction);

		// 위치 설정
		RectTransform boxRT = popup.transform.GetChild(0).GetComponent<RectTransform>();
		Canvas canvas = boxRT.GetComponentInParent<Canvas>();
		Util.SetPositionFromBottomRight(boxRT, 0f, 0f);
		Util.SetRelativeVerticalOffset(boxRT, canvas, 0.34f);
	}


}