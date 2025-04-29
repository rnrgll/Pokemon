using System;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    //pop up : 1회성 알림 -> 종료시 삭제
    //linked up : 계층 구조 UI -> 종료시 비활성화
    
    
    //rootUI : ui object의 최상의 부모(컨테이너)
    private GameObject _rootUI;
    //PopUp 용 스택
    [SerializeField] private Stack<UI_PopUp> _popUpStack = new Stack<UI_PopUp>();
    private int _order = 10;
    
    //Linked 용 스택
    [SerializeField] private List<UI_Linked> _linkList = new List<UI_Linked>();
    
    
    
    //ui가 열려있는지 여부
    public bool IsAnyUIOpen =>  _popUpStack.Count > 0 || _linkList.Count > 0;
    //ui가 열려있는지 여부에 따른 액션이벤트
    public event Action OnAllUIClosed;
    
    
    //임시 초기화
    private void Awake()
    {
        CreateInstance();
    }

    private void OnDestroy()
    {
	    OnAllUIClosed = null;
    }

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
        if(!IsAnyUIOpen)
	        OnAllUIClosed?.Invoke();
    }

    #endregion


    #region LinkedUI
    /// <summary>
    /// Linked UI를 show할 때, 현재 보이는 LinkedUI는 비활성화 -> 새로운 LinkedUI를 인스턴스화하여 띄운다 -> 리스트에 추가한다.
    /// </summary>
    /// <param name="prefabPath"></param>
    /// <typeparam name="T">Resources/UI_Prefabs/Linked 기준 상대경로</typeparam>
    /// <returns></returns>
    public T ShowLinkedUI<T>(string prefabPath) where T : UI_Linked
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
        
        linked.Open();

        
        return linked;
    }

    public void UndoLinkedUI()
    {
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

    #endregion
    

    
}
