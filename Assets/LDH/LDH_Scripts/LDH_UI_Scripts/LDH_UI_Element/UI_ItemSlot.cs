using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : PoolObject<UI_ItemSlot>
{
	[SerializeField] private TMP_Text tmCode;
	[SerializeField] private Image redArrow;
	[SerializeField] private TMP_Text itemName;
	[SerializeField] private Image xSymbol;
	[SerializeField] private TMP_Text itemCnt;

	public void Deselect()
	{
		SetVisible(redArrow, false);
	}

	public void Select()
	{
		SetVisible(redArrow, true);
	}

	public void SetData(InventorySlot slotData)
	{
		// 기본 텍스트 설정
		itemName.text = slotData.ItemName;
		itemCnt.text = slotData.Count.ToString();

		// 아이템 정보 가져오기
		ItemBase itemData = Manager.Data.ItemDatabase.GetItemData(slotData.ItemName);
		if (itemData == null)
		{
			Debug.LogWarning($"[UI_ItemSlot] 아이템 정보를 가져올 수 없습니다: {slotData.ItemName}");
			return;
		}

		bool isSkillMachine = itemData is Item_SkillMachine;
		bool isHM = false;
		string skillCode = "";
		
		if (isSkillMachine && itemData is Item_SkillMachine sm)
		{
			skillCode = sm.SkillCode;
			isHM = sm.MachineType == SkillMachineType.HM;
		}

		// 기술머신 / 비전머신이면 tmCode 활성화 및 설정
		SetVisible(tmCode, isSkillMachine);
		tmCode.text = skillCode;

		// 개수 및 x 심볼 표시 여부
		bool showCount =
			!(itemData.Category == Define.ItemCategory.KeyItem || isHM);

		SetVisible(xSymbol, showCount);
		SetVisible(itemCnt, showCount);
		itemCnt.text = showCount ? slotData.Count.ToString() : "";
	}

	#region 유틸리티 메서드

	private void SetVisible(Graphic ui, bool isVisible)
	{
		if (ui.TryGetComponent<CanvasGroup>(out var group))
		{
			group.alpha = isVisible ? 1f : 0f;
			group.interactable = isVisible;
			group.blocksRaycasts = isVisible;
		}
		else
		{
			ui.gameObject.SetActive(isVisible);
		}
	}
	

	private void SetVisible(CanvasGroup group, bool isVisible)
	{
		group.alpha = isVisible ? 1f : 0f;
		group.interactable = isVisible;
		group.blocksRaycasts = isVisible;
	}

	#endregion
}
