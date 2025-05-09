using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UIManager : Singleton<UIManager>
{
	//========================================//
    //pop up : 1회성 알림 -> 종료시 삭제
    //linked up : 계층 구조 UI -> 종료시 비활성화
    //========================================//

    #region Variables
    //rootUI : ui object의 최상의 부모(컨테이너)
    private GameObject _rootUI;

    //PopUp 용 스택
    private Stack<UI_PopUp> _popUpStack = new Stack<UI_PopUp>();
    private int _order = 50;
    private int _linkedDefaultOrder = 10;

    //Linked 용 스택
    [SerializeField] private List<UI_Linked> _linkList = new List<UI_Linked>();
    
    //UI Root
    public GameObject RootUI
    {
	    get
	    {
		    if (_rootUI == null)
		    {
			    _rootUI = GameObject.Find("UI_Root");
			    if (_rootUI == null)
			    {
				    _rootUI = new GameObject("UI_Root");
			    }
		    }

		    return _rootUI;
	    }
    }
    
    //ui가 열려있는지 여부
    public bool IsAnyUIOpen =>  _popUpStack.Count > 0 || _linkList.Count > 0;
    //ui가 열려있는지 여부에 따른 액션이벤트
    public event Action OnAllUIClosed;
    
    
    #endregion


    #region EventFunction
    
    private void OnDestroy()
    {
	    OnAllUIClosed = null;
    }

    private void Update()
    {
	    if(!IsAnyUIOpen) return;
	    if (Input.GetKeyDown(KeyCode.Z)) HandleUIInput(UIInputType.Select);
	    else if (Input.GetKeyDown(KeyCode.X)) HandleUIInput(UIInputType.Cancel);
	    else if (Input.GetKeyDown(KeyCode.UpArrow)) HandleUIInput(UIInputType.Up);
	    else if (Input.GetKeyDown(KeyCode.DownArrow)) HandleUIInput(UIInputType.Down);
	    else if(Input.GetKeyDown(KeyCode.RightArrow)) HandleUIInput(UIInputType.Right);
	    else if(Input.GetKeyDown(KeyCode.LeftArrow)) HandleUIInput(UIInputType.Left);

    }

    #endregion
    
    
    //캔버스 sorting order 셋팅
    public void SetCanvas(GameObject uiGameObject, bool isPopup = false)
    {
	    
        Canvas canvas = uiGameObject.GetComponent<Canvas>();
        
        //렌더 - 오버레이
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //override sorting - true
        canvas.overrideSorting = true;

        if (isPopup)
        {
            canvas.sortingOrder =_order;
            _order++;
        }
        else
        {
	        canvas.sortingOrder = _linkedDefaultOrder;
        }

    }
    
    
    //z키 선택 알림
    public void HandleUIInput(UIInputType inputType)
    {
	    if (_popUpStack.Count > 0)
	    {
		    var top = _popUpStack.Peek() as IUIInputHandler;
		    top?.HandleInput(inputType);
		    return;
	    }

	    if (_linkList.Count > 0)
	    {
		    var top = _linkList[^1] as IUIInputHandler;
		    top?.HandleInput(inputType);
	    }
	    // 둘 다 없으면 무시
    }
    

    #region 팝업 UI

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prefabPath">Resources/UI_Prefabs/PopUp 기준 상대경로</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T ShowPopupUI<T>(string prefabPath) where T : UI_PopUp
    {
        if (string.IsNullOrEmpty(prefabPath))
        {
            Debug.LogError("프리팹 경로를 지정해주세요.");
            return null;
        }

        GameObject prefab = Resources.Load<GameObject>($"UI_Prefabs/PopUp/{prefabPath}");

        if (prefab == null)
        {
            Debug.LogError("프리팹 경로 오류");
            return null;
        }

        T popUp = Instantiate(prefab,RootUI.transform).GetComponent<T>();
        _popUpStack.Push(popUp);
        
        
        return popUp;
    }

    public void ClosePopupUI(UI_PopUp popUp)
    {
        if (_popUpStack.Count == 0) return;
        if (_popUpStack.Peek() != popUp)
        {
            Debug.Log("팝업이 일치하지 않습니다.");
            return;
        }
        UI_PopUp stackPopUp = _popUpStack.Pop();
        Destroy(stackPopUp.gameObject);
        _order--;

        //모든 UI가 다 닫혔으면 이벤트 호출
        if (!IsAnyUIOpen)
        {
	        Debug.Log("팝업다다다다다다다닫");
	        OnAllUIClosed?.Invoke();
        }
	       
    }

    #endregion


    #region LinkedUI
    /// <summary>
    /// Linked UI를 show할 때, 현재 보이는 LinkedUI는 비활성화 -> 새로운 LinkedUI를 인스턴스화하여 띄운다 -> 리스트에 추가한다.
    /// </summary>
    /// <param name="prefabPath"></param>
    /// <typeparam name="T">Resources/UI_Prefabs/Linked 기준 상대경로</typeparam>
    /// <returns></returns>
    public T ShowLinkedUI<T>(string prefabPath, bool autoOpen = true) where T : UI_Linked
    {
        if (string.IsNullOrEmpty(prefabPath))
        {
            Debug.LogError("프리팹 경로를 지정해주세요.");
            return null;
        }
        GameObject prefab = Resources.Load<GameObject>($"UI_Prefabs/Linked/{prefabPath}");
        
        if (prefab == null)
        {
            Debug.LogError("프리팹 경로 오류");
            return null;
        }
        
        if (_linkList.Count > 0)
            _linkList[_linkList.Count-1].Close(); // 마지막 Linked UI(현재 보이는 UI)를 비활성화

        T linked = Instantiate(prefab, RootUI.transform).GetComponent<T>();
        linked.transform.SetAsLastSibling();
        _linkList.Add(linked); // 리스트에 추가
        
        if (autoOpen)
	        linked.Open(); // 기존처럼 바로 Open
        else
        {
	        linked.Close();
        }
        
        // Debug.Log(Manager.Game.Player.state.ToString());
        // Debug.Log(IsAnyUIOpen);
        
        return linked;
    }

    public void UndoLinkedUI()
    {
	    if (_popUpStack.Count != 0)
	    {
		    CloseAllPopUp();
	    }
	    
        if (_linkList.Count == 0)
            return;

        UI_Linked lastLinked = _linkList[_linkList.Count-1]; //마지막 UI = 현재 UI
        _linkList.RemoveAt(_linkList.Count - 1); //리스트에서 제거
        Destroy(lastLinked.gameObject); //삭제

        if (_linkList.Count > 0)
            _linkList[_linkList.Count-1].Open(); // 직전 UI 활성화

        
        //모든 UI가 다 닫혔으면 이벤트 호출
        if(!IsAnyUIOpen)
	        OnAllUIClosed?.Invoke();
    }

    public void CloseAllPopUp()
    {
	    while (_popUpStack.Count != 0)
	    {
		    ClosePopupUI(_popUpStack.Peek());
	    }
	    Debug.Log("열려있는 팝업을 모두 닫았습니다.");
    }

    public void CloseAllUI()
    {
	    while (_linkList.Count != 0)
	    {
		    UndoLinkedUI();
	    }
	    Debug.Log("열려있는 모든 UI를 닫았습니다.");
    }
    

    #endregion

    #region 자주 사용하는 팝업 ui 생성 메소드

    
    public void ShowConfirmPopup(Action onYes, Action onNo = null)
    {
	    //아니오 액션 저장
	    ISelectableAction noAction = new CustomAction(() => onNo?.Invoke());
		
	    var popup = Manager.UI.ShowPopupUI<UI_SelectPopUp>("UI_SelectablePopUp");
		
		

	    popup.SetupOptions(new()
	    {
		    ("예", new CustomAction(() => {
			    onYes?.Invoke();
		    })),
		    ("아니오", noAction)
	    });
		
	    popup.OverrideCancelAction(noAction);
	    popup.gameObject.SetActive(false);
	    // 위치 설정
	    RectTransform boxRT = popup.transform.GetChild(0).GetComponent<RectTransform>();
	    Canvas canvas = boxRT.GetComponentInParent<Canvas>(true);
	    Util.SetPositionFromBottomRight(boxRT, 0f, 0f);
	    Util.SetRelativeVerticalOffset(boxRT, canvas, 0.34f);
	    
	    popup.gameObject.SetActive(true);
    }

    
    public void ShowCountPopup(int maxAmount, Action<int> onConfirm, Action onCancel)
    {
	    var countUI = Manager.UI.ShowPopupUI<UI_CountPopUp>("UI_CountPopUp");
	    countUI.Init(
		    maxAmount, onConfirm, onCancel
	    );
		
	    countUI.gameObject.SetActive(false);
		
	    RectTransform boxRT = countUI.transform.GetChild(0).GetComponent<RectTransform>();
	    Canvas canvas = boxRT.GetComponentInParent<Canvas>(true);
		
	    Util.SetPositionFromBottomRight(boxRT, 0f, 0f);
	    Util.SetRelativeVerticalOffset(boxRT,canvas,0.34f);
	    countUI.gameObject.SetActive(true);
    }


    public void ShowDialogPopUp(List<string> lines, Action onFinished = null, bool needNextButton = true,  bool useTypingEffect = false, bool dontClose = false)
    {
	    Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp").ShowMessage(lines,onFinished, needNextButton, useTypingEffect, dontClose);
	    
    }

    #endregion
}
