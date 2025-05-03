using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GenericButton : MonoBehaviour
{
	[SerializeField] protected TMP_Text _label;
	[SerializeField] protected Image _arrow;
	
	private ISelectableAction _selectableAction;
	

	protected virtual void Awake()
	{
		_arrow??= transform.GetChild(0).GetComponent<Image>();
		_label??=transform.GetChild(1).GetComponent<TMP_Text>();
	}
	
	public void SetAction(ISelectableAction action)
	{
		_selectableAction = action;
	}
	public virtual void Trigger()
	{
		_selectableAction?.Execute();
	}
	
	
	private void OnDisable()
	{
		SetArrowActive(false);
	}


	public void SetArrowActive(bool active)
	{
		Util.SetVisible(_arrow,active);
	}
	
	
	
	// public void Trigger()
	// {
	// 	_menuAction?.Execute();
	// }
	//
	//
	// public void SetArrowActive(bool active)
	// {
	// 	if (_arrow != null)
	// 		Util.SetVisible(_arrow, active);
	// }
	//
	// // public void SetAction(Action action)
	// // {
	// // 	_onSelectAction = action;
	// // }
	// //

}
