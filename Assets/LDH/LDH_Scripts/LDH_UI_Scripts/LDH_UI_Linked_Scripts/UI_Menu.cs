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
    
    [Header("UI")]
    [SerializeField] private Transform _UI_MenuBox;
    [SerializeField] private Transform _UI_WhiteBox;
    [SerializeField] private List<UIMenuSelectButton> menuBtns;
    
    private TMP_Text _UI_WhiteBoxText;
    
	private List<UIMenuSelectButton> _activeMenuButtons = new();
	private Dictionary<string, ISelectableAction> menuAction;
	
   
    private void Awake()
    {
	    _UI_WhiteBox = transform.GetChild(0);
	    _UI_MenuBox = transform.GetChild(1);
	    
	    _UI_WhiteBoxText = _UI_WhiteBox.GetComponentInChildren<TMP_Text>();
	    
	    menuAction = new()
	    {
		   ["포켓몬"] = new OpenLinkedUIAction("UI_PokemonParty"),
		   ["가방"] = new OpenLinkedUIAction("UI_Bag"),
		   ["포켓기어"] = new CustomAction(() => Debug.Log("선택됨. 구현되지 않은 기능")),
		   ["플레이어"] = new OpenLinkedUIAction("UI_PlayerCard"),
		   ["레포트"] = new CustomAction(() => Debug.Log("선택됨. 구현되지 않은 기능")),
		   ["설정"] = new CustomAction(() => Debug.Log("선택됨. 구현되지 않은 기능")),
		   ["닫다"] = new CustomAction(() => CloseSelf()),
	    };
	       
	    // SetupOptions( _UI_MenuBox,
		   //  new List<(string, ISelectableAction)>
	    // {
		   //  ("포켓몬", new OpenLinkedUIAction("UI_PokemonParty")),
		   //  ("가방", new OpenLinkedUIAction("UI_Bag")),
		   //  ("포켓기어", new CustomAction(() => Debug.Log("선택됨. 구현되지 않은 기능"))),
		   //  ("이름", new OpenLinkedUIAction("UI_PlayerCard")),
		   //  ("레포트",new CustomAction(() => Debug.Log("선택됨. 구현되지 않은 기능"))),
		   //  ("설정", new CustomAction(() => Debug.Log("선택됨. 구현되지 않은 기능"))),
		   //  ("닫다", new CustomAction(() => CloseSelf())),
	    // });
	    
    }


    private void SetMenu()
    {
	    _activeMenuButtons.Clear();
	    for (int i = 0; i < menuBtns.Count; i++)
	    {
		    string menuName = menuBtns[i].MenuName;
		    bool isOpen = Manager.Data.PlayerData.menuFlag[menuName];
		    if (isOpen)
		    {
			    menuBtns[i].SetAction(menuAction[menuName]);
			    _activeMenuButtons.Add(menuBtns[i]);
		    }
		    else
		    {
			    menuBtns[i].gameObject.SetActive(false);
		    }
	    }
    }
  

    private void OnEnable()
    {
	    SetMenu();
	    UpdateUI();
    }

    protected override void Init()
    {
	    base.Init();
	    Manager.Game.SetSlotType(UI_PokemonParty.PartySlotType.Menu);
	    SetInActiveAllArrow();
		UpdateUI();
    }


    public override void HandleInput(Define.UIInputType inputType)
    {
	    
	    switch (inputType)
	    {
		    case Define.UIInputType.Up:
			    MoveIdx(-1);
			    break;
		    case Define.UIInputType.Down:
			    MoveIdx(1);
			    break;
		    case Define.UIInputType.Select:
			    OnSelect();
			    break;
		    case Define.UIInputType.Cancel:
			    OnCancel();
			    break;
	    }
    }

    // void RefreshActiveMenuList()
    // {
	   //  _activeMenuButtons.Clear();
	   //  foreach (UIMenuSelectButton btn in menuBtns)
	   //  {
		  //   UIMenuSelectButton menuSelectButton = child.GetComponent<UIMenuSelectButton>();
		  //   menuSelectButton.SetOpened(Manager.Instance.PlayerData.menuFlag[menuSelectButton.GetMenuName()]);
    //
		  //   if (menuSelectButton.IsOpened)
		  //   {
			 //    _activeMenuButtons.Add(menuSelectButton);
			 //    
		  //   }
	   //  }
	   //  Debug.Log(_activeMenuButtons.Count);
    // }

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
	    if (_activeMenuButtons.Count == 0) return;
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
	    foreach (UIMenuSelectButton activeMenuButton in _activeMenuButtons)
	    {
		    activeMenuButton.SetArrowActive(false);
	    }
    }

    #endregion
   

    
    public override void OnSelect()
    {
	    _activeMenuButtons[_curIdx].Trigger();
    }


   
}
