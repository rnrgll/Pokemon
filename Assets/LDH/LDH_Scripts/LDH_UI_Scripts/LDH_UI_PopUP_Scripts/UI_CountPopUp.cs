using System;
using TMPro;
using UnityEngine;
public class UI_CountPopUp : UI_PopUp
{
	[SerializeField] private TMP_Text countText;
	private int _maxCount;
	private int _curCount = 1;

	private Action<int> _onConfirm;
	private Action _onCancel;

	public void Init(int max, Action<int> onConfirm, Action onCancel)
	{
		_maxCount = max;
		_curCount = 1;
		_onConfirm = onConfirm;
		_onCancel = onCancel;
		RefreshCountUI();
	}

	private void RefreshCountUI()
	{
		countText.text = $"x{_curCount:D2}";
	}

	private void AdjustCount(bool increase)
	{
		_curCount += increase ? 1 : -1;
		if (_curCount <= 0) _curCount = _maxCount;
		else if (_curCount > _maxCount) _curCount = 1;
		RefreshCountUI();
	}

	public override void HandleInput(Define.UIInputType inputType)
	{
		switch (inputType)
		{
			case Define.UIInputType.Up:
				AdjustCount(true);
				break;
			case Define.UIInputType.Down:
				AdjustCount(false);
				break;
			case Define.UIInputType.Select:
				OnSelect();
				break;
			case Define.UIInputType.Cancel:
				OnCancel();
				break;
		}
	}

	public override void OnSelect()
	{
		base.OnSelect();
		_onConfirm?.Invoke(_curCount);
		
	}
	
	public override void OnCancel()
	{
		_onCancel?.Invoke();
		base.OnCancel();
	}
}
