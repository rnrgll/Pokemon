using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BattleMenuController : MonoBehaviour
{
	[SerializeField] private Transform buttonRoot;
	
    //인덱스 반영
    //0  1   
    //2  3
    private List<List<UI_GenericSelectButton>> menuButtonGrid;
    public List<List<UI_GenericSelectButton>> MenuButtonGrid => menuButtonGrid;
    private int curX = 0;
    private int curY = 0;

    private void Awake()
    {
	    //하위 자식들을 리스트에 넣어준다.
	    menuButtonGrid = new();
	    for (int i = 0; i < 2; i++) // y축 (행)
	    {
		    List<UI_GenericSelectButton> row = new();
		    for (int j = 0; j < 2; j++) // x축 (열)
		    {
			    row.Add(buttonRoot.GetChild(2*i+j).GetComponent<UI_GenericSelectButton>());
		    }
		    menuButtonGrid.Add(row);
	    }
    }

    private void OnEnable()
    {
	    curX = 0;
	    curY = 0;
	    //Debug.Log($"<color=yellow>{curY}, {curX}</color>");
	    menuButtonGrid[curY][curX].SetArrowActive(true);
    }

    private void Update()
    {
	    if(Manager.UI.IsAnyUIOpen) return;
	    
	    if (Input.GetKeyDown(KeyCode.RightArrow)) MoveCursor(1, 0);
	    else if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveCursor(-1, 0);
	    else if (Input.GetKeyDown(KeyCode.UpArrow)) MoveCursor(0, -1);
	    else if (Input.GetKeyDown(KeyCode.DownArrow)) MoveCursor(0, 1);
	    else if(Input.GetKeyDown(KeyCode.Z)) OnSelect();

    }

    private void MoveCursor(int dx, int dy)
    {
	    int x = Mathf.Clamp(curX + dx, 0, 1);
	    int y = Mathf.Clamp(curY + dy, 0, 1);
	    
	    menuButtonGrid[curY][curX].SetArrowActive(false);
	    curX = x;
	    curY = y;
	    menuButtonGrid[curY][curX].SetArrowActive(true);
    }

    public void OnSelect()
    {
	    this.gameObject.SetActive(false);
	    menuButtonGrid[curY][curX].Trigger();
	    
    }
    
    

}
