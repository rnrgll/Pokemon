using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UI_Menu : UI_Linked
{
    private static int _curIdx = 0;
    private int _preIdx = 0;
    
    [Header("UI Layout")]
    [SerializeField] private Transform _UI_MenuBox;
    [SerializeField] private Transform _UI_WhiteBox;
    private TMP_Text _UI_WhiteBoxText;
    [SerializeField] private List<UI_MenuButton> menuButtonsList;
    
    
   private List<UI_MenuButton> _activeMenuButtons = new();
   
    private void Awake()
    {
	    _UI_WhiteBox = transform.GetChild(0);
	    _UI_MenuBox = transform.GetChild(1);
	    
	    _UI_WhiteBoxText = _UI_WhiteBox.GetComponentInChildren<TMP_Text>();
       
	    SetupOptions(new List<(string, ISelectableAction)>
	    {
		    ("포켓몬", new OpenLinkedUIAction("UI_PokemonParty")),
		    ("가방", new OpenLinkedUIAction("UI_Bag")),
		    ("포켓기어", new CustomAction(() => Debug.Log("선택됨. 구현되지 않은 기능"))),
		    ("이름", new OpenLinkedUIAction("UI_Bag")),
		    ("레포트",new CustomAction(() => Debug.Log("선택됨. 구현되지 않은 기능"))),
		    ("설정", new CustomAction(() => Debug.Log("선택됨. 구현되지 않은 기능"))),
		    ("닫다", new CustomAction(() => Debug.Log("선택됨. 구현되지 않은 기능"))),
	    });
        
    }

    private void OnEnable()
    {
	    RefreshActiveMenuList();
	    UpdateUI();
    }

    protected override void Init()
    {
	    base.Init();
	    SetInActiveAllArrow();
		UpdateUI();
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
    }

    void RefreshActiveMenuList()
    {
	    _activeMenuButtons.Clear();
	    foreach (Transform child in _UI_MenuBox)
	    {
		    UI_MenuButton menuButton = child.GetComponent<UI_MenuButton>();
		    if (menuButton.IsOpened)
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
        
       UpdateUI();
    }


    #region UI

    void UpdateUI()
    {
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
    
    void SetInActiveAllArrow()
    {
	    foreach (UI_MenuButton activeMenuButton in _activeMenuButtons)
	    {
		    activeMenuButton.SetArrowActive(false);
	    }
    }

    #endregion
   

    
    public override void OnSelect()
    {
	    
	    _activeMenuButtons[_curIdx].Trigger();
	    // if (_curIdx == _activeMenuButtons.Count - 1)
	    // {
		   //  //'닫다' 메뉴
		   //  CloseSelf();
		   //  return;
	    // }
	    //
	    // //그 외 메뉴는 각자 UI 띄우기
	    // UI_MenuButton selectedButton = _activeMenuButtons[_curIdx];
	    // selectedButton.OpenMenu();
	    
    }


    public void SetupOptions(List<(string label, ISelectableAction action)> options)
    {
	    _activeMenuButtons.Clear();
	    for (int i = 0; i < _UI_MenuBox.childCount; i++)
	    {
		    UI_MenuButton menuButton = _UI_MenuBox.GetChild(i).GetComponent<UI_MenuButton>();
		    
		    if (menuButton.IsOpened)
		    {
			    _activeMenuButtons.Add(menuButton);
			    
		    }
		    
		    menuButton.SetAction(options[i].action);
		    
		    //menuButton.SetArrowActive(false);
		    
	    }
    }

    
}
