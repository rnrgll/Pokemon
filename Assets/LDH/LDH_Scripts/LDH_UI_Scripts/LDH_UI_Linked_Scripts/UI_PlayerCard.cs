using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PlayerCard : UI_Linked
{
	[SerializeField] private GameObject[] infoPanels;
	[SerializeField] private int curIdx;
	
	private void Awake()
	{
		if (infoPanels == null || infoPanels.Length == 0)
		{
			infoPanels = new GameObject[2];
			infoPanels[0] = transform.GetChild(0).gameObject;
			infoPanels[1] = transform.GetChild(1).gameObject;
		}

		curIdx = 0;
	}

    private void OnEnable()
    {
	    UpdateInfoPanels();
    }



    public override void HandleInput(Define.UIInputType inputType)
    {
	    switch (inputType)
	    {
		    case Define.UIInputType.Left:
			    curIdx = Mathf.Clamp(curIdx - 1, 0, infoPanels.Length - 1);
			    UpdateInfoPanels();
			    break;
		    case Define.UIInputType.Right:
			    curIdx = Mathf.Clamp(curIdx + 1, 0, infoPanels.Length - 1);
			    UpdateInfoPanels();
			    break;
		    case Define.UIInputType.Select:
			    OnSelect();
			    break;
		    case Define.UIInputType.Cancel:
			    OnCancel();
			    break;
	    }

	    
    }


    private void UpdateInfoPanels()
    {
	    for (int i = 0; i < infoPanels.Length; i++)
	    {
		    infoPanels[i].SetActive(i == curIdx);
	    }
    }

    
    //해당 UI는 선택 항목 처리가 필요 없는 화면이므로 OnSelect()는 오버라이드하지 않음
    // OnCancle()은 UI_Linked의 기본 닫기 로직(CloseSelf)을 그대로 사용
}
