using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
	[SerializeField] private TMP_Text tmCode;
	[SerializeField] private Image redArrow;
	[SerializeField] private TMP_Text itemName;
	[SerializeField] private Image xSymbol;
	[SerializeField] private TMP_Text itemCnt;
	

    public void Deselect()
    {
	    SetVisible(redArrow.GetComponent<CanvasGroup>(),false);
    }

    public void Select() //선택이라기보다 해당 아이템 슬롯으로 커서가 옮겨갔을때
    {
	    SetVisible(redArrow.GetComponent<CanvasGroup>(),true);
    }
    
    
    private void SetVisible(CanvasGroup group, bool isVisible)
    {
	    group.alpha = isVisible ? 1f : 0f;
	    group.interactable = isVisible;
	    group.blocksRaycasts = isVisible;
    }
}
