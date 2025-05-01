using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{ 
	[SerializeField] GameObject dialogBox;
	//	NPC 텍스트
	[SerializeField] TMP_Text dialogText;
	// 타이핑 시간
	[SerializeField] int letterPerSec;

	//	가져올 프리펩의 경로
	GameObject prefab = Resources.Load<GameObject>("MSK/MSKPrefab/DialogPrefab}");


	Dialog dialog;

	// NPC대사 단위
	int currentLine = 0;
	//	대사 출력의 여부
	public bool isTyping;

	public static DialogManager Instance { get; private set; }
	
	
	public event Action OnShowDialog;
	public event Action CloseDialog;

	protected override void Awake()
	{
		Instance = this;
	}

	public void HandleUpdate()
	{	// 대화 진행중
		if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
		{	// 다음 대사박스 출력
			++currentLine;
			if (currentLine < dialog.Lines.Count)
			{
				StartCoroutine(ShowDialog(dialog.Lines[currentLine]));
			}
			else
			{
				currentLine = 0;
				dialogBox.SetActive(false);
				CloseDialog?.Invoke();
			}
		}
	}


	public IEnumerator ShowText(Dialog dialog)
	{
		yield return new WaitForEndOfFrame();
		OnShowDialog?.Invoke();

		this.dialog = dialog;
		dialogBox.SetActive(true);
		dialogText.text = dialog.Lines[0];
		StartCoroutine(ShowDialog(dialog.Lines[0]));
	}



	public IEnumerator ShowDialog(string dialog)
	{
		isTyping = true;
		dialogText.text = "";
		foreach (var letter in dialog.ToCharArray())
		{
			dialogText.text += letter;
			yield return new WaitForSeconds(0.5f / letterPerSec);
		}
		isTyping = false;
	}
}
