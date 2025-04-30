using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PlayerCard : UI_Linked, IUISelectable
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


    private void Update()
    {
	    if (Input.GetKeyDown(KeyCode.LeftArrow))
	    {
		    curIdx = Mathf.Clamp(curIdx - 1, 0, infoPanels.Length - 1);
		    UpdateInfoPanels();
	    }
	    else if (Input.GetKeyDown(KeyCode.RightArrow))
	    {
		    curIdx = Mathf.Clamp(curIdx + 1, 0, infoPanels.Length - 1);
		    UpdateInfoPanels();
	    }
    }


    private void UpdateInfoPanels()
    {
	    for (int i = 0; i < infoPanels.Length; i++)
	    {
		    infoPanels[i].SetActive(i == curIdx);
	    }
    }

    public void OnSelect()
    {
	    //Status Panel에서 선택 입력 시, Badge Panel로 넘어감
	    if (curIdx == 0)
	    {
		    curIdx = 1;
		    UpdateInfoPanels();

	    }

	    //Badge Panel에서 선택 입력 시, 메뉴 종료됨
	    else
	    {
		    Manager.UI.UndoLinkedUI();
	    }
    }
}
