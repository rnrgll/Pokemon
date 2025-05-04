using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UI_MultiLinePopUp :UI_PopUp, IUIInputHandler
{
	[SerializeField] private UI_DialogueNextButton nextButton;
	[SerializeField] private TMP_Text messageText;

	private List<string> _lines;
	private int _curIndex;
	private Action _onFinished;

	private bool _canReceiveInput = false;
	
	private Coroutine _displayCoroutine;
	

	public void ShowMessage(List<string> lines, Action onFinished = null)
	{
		_lines = lines;
		_curIndex = 0;
		_onFinished = onFinished;
		
		
		if (_displayCoroutine != null)
			StopCoroutine(_displayCoroutine);

		_displayCoroutine = StartCoroutine(DisplayRoutine());
	}

	private IEnumerator DisplayRoutine()
	{
		_canReceiveInput = false;
		messageText.text = string.Empty;
		for (int i = 0; i < 2; i++)
		{
			int lineIdx = _curIndex + i;
			if (lineIdx < _lines.Count)
			{
				// 타이핑 효과 대용: 한 줄씩 딜레이 출력 (옵션)
				messageText.text += _lines[lineIdx] + "\n";
				
			}
		}
		yield return new WaitForSeconds(0.5f); // 지연
		
		// 출력 완료 후 버튼 활성화 및 입력 허용
		nextButton.gameObject.SetActive(true);
		_canReceiveInput = true;
		
		_displayCoroutine = null;
	}
	
	
	public void HandleInput(Define.UIInputType inputType)
	{
		if (!_canReceiveInput) return;
		if (inputType != Define.UIInputType.Select && inputType != Define.UIInputType.Cancel) 
			return;
		
		// 다음 대사로 넘기기
		_curIndex += 2;
		if (_curIndex >= _lines.Count)
		{
			Quit();
		}
		else
		{
			nextButton.gameObject.SetActive(false);
			StartCoroutine(DisplayRoutine());
		}
		
	}

	private void Quit()
	{
		nextButton.gameObject.SetActive(false);
		_onFinished?.Invoke();
		base.OnSelect(); 
	}
	
}
