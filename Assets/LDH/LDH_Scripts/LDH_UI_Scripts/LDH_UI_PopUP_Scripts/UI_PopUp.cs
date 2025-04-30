using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PopUp : MonoBehaviour, IUISelectable, ICancelable
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
	    //일단 아무 기능 없이 둠
	    //추후 구현 예정
    }
    public virtual void OnCancle()
    {
	    ClosePopupUI();
    }
    
}
