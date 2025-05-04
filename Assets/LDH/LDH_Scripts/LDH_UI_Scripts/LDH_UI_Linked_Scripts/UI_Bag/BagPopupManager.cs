using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BagPopupManager
{
	private readonly UI_Bag _bag;

	public BagPopupManager(UI_Bag bag)
	{
		_bag = bag;
	}

	public void ShowItemActionPopup(ItemBase item)
	{
		bool isBattle = SceneManager.GetActiveScene().name == "BattleScene";

		bool? canUse = item?.CanUseNow(InGameContextFactory.CreateBasic(isBattle));

		var popup = Manager.UI.ShowPopupUI<UI_SelectPopUp>("UI_SelectablePopUp");

		if (canUse == true)
		{
			popup.SetupOptions(new()
			{
				("사용하다", new CustomAction(() => Debug.Log("아이템 사용"))),
				("버리다", new CustomAction(() => Debug.Log("아이템 버리기"))),
				("그만두다", new CustomAction(popup.OnCancle))
			});
		}
		else
		{
			popup.SetupOptions(new()
			{
				("그만두다", new CustomAction(popup.OnCancle))
			});
		}

		SetPopupPosition(popup);
	}

	private void SetPopupPosition(UI_SelectPopUp popup)
	{
		RectTransform boxRT = popup.transform.GetChild(0).GetComponent<RectTransform>();

		boxRT.anchorMin = new Vector2(0f, 0f);
		boxRT.anchorMax = new Vector2(0f, 0f);
		boxRT.pivot = new Vector2(0f, 0f);

		float canvasHeight = ((RectTransform)popup.transform).rect.height;
		float y = canvasHeight * 0.34f;

		boxRT.anchoredPosition = new Vector2(0f, y);
	}
}