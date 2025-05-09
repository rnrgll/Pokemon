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

		//배틀 여부 체크
		bool isBattleScene = Manager.Game.IsInBattle;
		
		
		// 팝업 생성 및 옵션 설정
		var popup = Manager.UI.ShowPopupUI<UI_SelectPopUp>("UI_SelectablePopUp");
	
		popup.OverrideCancelAction(new CustomAction(() =>
		{
			_bag.Refresh();
		}));
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
					var options = new List<(string, ISelectableAction)>
					{
						("사용하다", new CustomAction(() => _bag.StartUseFlow(slot)))
					};
					if (!isBattleScene)
					{
						options.Add(("버리다", new CustomAction(() => _bag.StartDropFlow(slot))));

					}
					options.Add(("그만두다", new CustomAction(popup.OnCancel)));
					popup.SetupOptions(options);
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
		popup.gameObject.SetActive(false);
		RectTransform boxRT = popup.transform.GetChild(0).GetComponent<RectTransform>();
		Canvas canvas = boxRT.GetComponentInParent<Canvas>(true);
		Util.SetPositionFromBottomLeft(boxRT, 0f, 0f);
		Util.SetRelativeVerticalOffset(boxRT,canvas,0.34f);
		popup.gameObject.SetActive(true);
	}



}