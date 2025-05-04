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

	public void ShowItemActionPopup(ItemBase item)
	{
		// 현재 전투 중인지 확인 (아이템 사용 가능 여부 판단에 필요)
		bool isBattle = SceneManager.GetActiveScene().name == "BattleScene";

		bool? canUse = item?.CanUseNow(InGameContextFactory.CreateBasic(isBattle));

		// 팝업 생성 및 옵션 설정
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

	
	// 팝업 UI의 시각적 위치 조정
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