using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	private bool _needNextButton;
	
	[SerializeField] private float typingSpeed = 0.5f; // 타이핑 속도 (글자당 지연 시간)
	private bool _useTypingEffect = false;

	public void ShowMessage(List<string> lines, Action onFinished = null, bool needNextButton = true,  bool useTypingEffect = false)
	{
		_lines = lines;
		_curIndex = 0;
		_needNextButton = needNextButton;
		_onFinished = onFinished;
		_useTypingEffect = useTypingEffect;
		
		if (_displayCoroutine != null)
			StopCoroutine(_displayCoroutine);

		_displayCoroutine = StartCoroutine(DisplayRoutine());
	}

	public void ShowMessage(string line, Action onFinished = null, bool needNextButton = true,   bool useTypingEffect = false)
	{
		List<string> lines = line.Split("\n").ToList();
		foreach (string splitline in lines)
		{
			Debug.LogFormat($"<color=yellow>{splitline}</color>");
		}
		ShowMessage(lines, onFinished, needNextButton, useTypingEffect);
	}

	private IEnumerator DisplayRoutine()
	{
		_canReceiveInput = false;
		messageText.text = string.Empty;
		
		int lineCount = 2;
		for (int i = 0; i < lineCount; i++)
		{
			int lineIdx = _curIndex + i;
			if (lineIdx < _lines.Count)
			{
				string line = _lines[lineIdx];
				if (_useTypingEffect)
				{
					//한자씩 출력
					foreach (char c in line)
					{
						messageText.text += c;
						yield return new WaitForSeconds(typingSpeed);
					}

					messageText.text += '\n';
				}

				else
				{
					//그냥 출력
					messageText.text += _lines[lineIdx] + "\n";
				}
				
				
				
			}
		}
		yield return new WaitForSeconds( _useTypingEffect? 0.2f : 0.5f); // 지연

		if (_needNextButton)
		{
			// 출력 완료 후 버튼 활성화
			nextButton.gameObject.SetActive(true);
		}
		 // 입력 허용
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
		base.OnSelect(); 
		nextButton.gameObject.SetActive(false);
		_onFinished?.Invoke();
		
	}
	
}
