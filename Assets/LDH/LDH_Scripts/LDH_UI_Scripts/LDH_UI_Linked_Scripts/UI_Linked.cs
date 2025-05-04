using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Linked : MonoBehaviour, IUIInputHandler, IUISelectable, IUICancelable
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

    
    public virtual void OnCancel()
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

    public virtual void HandleInput(Define.UIInputType inputType)
    {
	  
    }
}
