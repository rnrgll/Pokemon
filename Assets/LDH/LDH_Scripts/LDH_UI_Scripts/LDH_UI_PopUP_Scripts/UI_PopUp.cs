using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class UI_PopUp : MonoBehaviour, IUIInputHandler, IUISelectable, IUICancelable
{
	// UI_PopUp.cs 내부

	[SerializeField] protected UI_GenericSelectButton buttonPrefab;
	[SerializeField] protected Transform buttonParent;
	public Transform ButtonParent => buttonParent;
	protected readonly List<UI_GenericSelectButton> _buttonList = new();
	
	
    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        Manager.UI.SetCanvas(gameObject,true);
    }


	public virtual void ClosePopupUI()
    {
        Manager.UI.ClosePopupUI(this);
        //Debug.Log(this.gameObject.name);
    }

    public virtual void OnSelect()
    {
	    ClosePopupUI();
    }
    public virtual void OnCancel()
    {
	    ClosePopupUI();
    }
    
    

    
    public void SetupOptions(Transform selectButtonParent, List<(string label, ISelectableAction action)> options)
    {
	    
	    // List<UI_GenericSelectButton> selectButtons
	    for (int i = 0; i < selectButtonParent.childCount; i++)
	    {
		    if (selectButtonParent.GetChild(i).TryGetComponent<UI_GenericSelectButton>(out var selectButton))
		    {
			    selectButton.SetAction(options[i].action);
		    }
		   
	    }
    }
    
    /// <summary>
    /// SetUpOptions 오버로딩, 각각 버튼의 텍스트와 기능 초기화가 필요한 경우
    /// </summary>
    /// <param name="options"></param>
    public virtual void SetupOptions(List<(string label, ISelectableAction action)> options)
    {
	    // 부족하면 생성
	    while (_buttonList.Count < options.Count)
	    {
		    var newButton = Instantiate(buttonPrefab, buttonParent);
		    newButton.gameObject.SetActive(false);
		    _buttonList.Add(newButton);
	    }

	    // 필요한 만큼 설정 및 활성화
	    for (int i = 0; i < options.Count; i++)
	    {
		    _buttonList[i].Init(options[i].label, options[i].action);
		    _buttonList[i].gameObject.SetActive(true);
	    }

	    // 나머지 비활성화
	    for (int i = options.Count; i < _buttonList.Count; i++)
	    {
		    _buttonList[i].gameObject.SetActive(false);
	    }
    }



    public virtual  void HandleInput(Define.UIInputType inputType)
    {

    }

    
}
