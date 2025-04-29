using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UI_Menu : UI_Linked, IUISelectable
{
    private static int _curIdx = 0;
    private int _preIdx = 0;
    
    [Header("UI Layout")]
    [SerializeField] private Transform _UI_MenuBox;
    [SerializeField] private Transform _UI_WhiteBox;
    private TMP_Text _UI_WhiteBoxText;
    
   [SerializeField] private List<UI_MenuButton> _activeMenuButtons;
   
    private void Awake()
    {
	    _UI_WhiteBox = transform.GetChild(0);
	    _UI_MenuBox = transform.GetChild(1);
	    
	    _UI_WhiteBoxText = _UI_WhiteBox.GetComponentInChildren<TMP_Text>();
       
        
    }

    private void OnEnable()
    {
	    RefreshActiveMenuList();
    }

    void Start()
    {
        UpdateArrow();
        UpdateMenuDescription();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveIdx(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
           MoveIdx(1);
        }
        else if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CloseSelf();
        }
    }

    void RefreshActiveMenuList()
    {
	    _activeMenuButtons.Clear();
	    foreach (Transform child in _UI_MenuBox)
	    {
		    UI_MenuButton menuButton = child.GetComponent<UI_MenuButton>();
		    if (menuButton.IsOpend)
		    {
			    _activeMenuButtons.Add(menuButton);
			    
		    }
	    }
    }

    void MoveIdx(int direction)
    {
        _preIdx = _curIdx;
        _curIdx += direction;
        if (_curIdx < 0)
            _curIdx = _activeMenuButtons.Count - 1;
        else if (_curIdx >= _activeMenuButtons.Count)
            _curIdx = 0;
        
        UpdateArrow();
        UpdateMenuDescription();
    }
    
    void UpdateArrow()
    { 
	    _activeMenuButtons[_preIdx].SetArrowActive(false);
		_activeMenuButtons[_curIdx].SetArrowActive(true);
    }

    void UpdateMenuDescription()
    {
	    _UI_WhiteBoxText.text = _activeMenuButtons[_curIdx].GetMenuDescription();
    }

    void CloseSelf()
    {
        Manager.UI.UndoLinkedUI();
    }

    public void OnSelect()
    {
	    if (_curIdx == _activeMenuButtons.Count - 1)
	    {
		    //'닫다' 메뉴
		    CloseSelf();
		    return;
	    }
	    
	    //그 외 메뉴는 각자 UI 띄우기
	    UI_MenuButton selectedButton = _activeMenuButtons[_curIdx];
	    selectedButton.OpenMenu();
	    
	    
    }
}
