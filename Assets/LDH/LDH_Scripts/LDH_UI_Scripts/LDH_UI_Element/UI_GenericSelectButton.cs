using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GenericSelectButton : MonoBehaviour
{
	[SerializeField] protected TMP_Text _label;
	[SerializeField] protected Image _arrow;
	
	private ISelectableAction _selectableAction;
	

	protected virtual void Awake()
	{
		_arrow??= transform.GetChild(0).GetComponent<Image>();
		_label??=transform.GetChild(1).GetComponent<TMP_Text>();
	}
	
	
	/// <summary>
	/// 버튼 텍스트와 콜백 함수 초기화가 필요한 경우에 호출
	/// </summary>
	/// <param name="label"></param>
	/// <param name="action"></param>
	public void Init(string label, ISelectableAction action)
	{
		_label.text = label;
		_selectableAction = action;
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
	
	
}
