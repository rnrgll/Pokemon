using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI_PopUp : MonoBehaviour, IUIInputHandler, IUISelectable, IUICancelable
{
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
    }

    public virtual void OnSelect()
    {
	    ClosePopupUI();
    }
    public virtual void OnCancle()
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

    public virtual  void HandleInput(Define.UIInputType inputType)
    {

    }
    
 
}
