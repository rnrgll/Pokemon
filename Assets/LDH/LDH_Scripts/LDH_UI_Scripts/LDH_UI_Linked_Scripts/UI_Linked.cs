using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Linked : MonoBehaviour, IUISelectable, ICancelable
{
    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        Manager.UI.SetCanvas(gameObject,false);
    }
    
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnCancle()
    {
	   CloseSelf();
    }


    protected void CloseSelf()
    {
	    Manager.UI.UndoLinkedUI();
    }

    //상속받은 쪽에서 구현하라고 처리
    public virtual void OnSelect()
    {
    }
}
