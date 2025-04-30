using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{
	// 타이핑 시간
	[SerializeField] int letterPerSec;

	//	가져올 프리펩의 경로

	[SerializeField] TMP_Text dialogText;
	[SerializeField] GameObject prefab;
	[SerializeField] GameObject dialogBox;
	GameObject dialogInstance = null;



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
	{   // 대화 진행중

		if (Input.GetKeyDown(KeyCode.Z))
		{
			
			// 다음 대사박스 출력
			if (!isTyping)
			{
				++currentLine;
				if (currentLine < dialog.Lines.Count)
				{
					StartCoroutine(ShowDialog(dialog.Lines[currentLine]));
				}
				else
				{
					currentLine = 0;
					dialogBox.SetActive(false);
					//--------------------//
					Manager.Game.Player.state = Define.PlayerState.Field;
					//--------------------//
					CloseDialog?.Invoke();
				}
			}
		}
	}


	public void StartDialogue(Dialog dialog)
	{
		Manager.Game.Player.state = Define.PlayerState.Dialog;
		CreateDialogueUI();
		StartCoroutine(DialogManager.Instance.ShowText(dialog));
	}


	private IEnumerator ShowText(Dialog dialog)
	{
		yield return new WaitForEndOfFrame();
		OnShowDialog?.Invoke();
		
		this.dialog = dialog;
		dialogBox.SetActive(true);
		dialogText.text = dialog.Lines[0];
		StartCoroutine(ShowDialog(dialog.Lines[0]));
	}

	private void CreateDialogueUI()
	{
		if (dialogInstance == null)
		{
			dialogInstance = Instantiate(prefab);
			// 인스턴스 내부 트랜스폼을 통하여 프리팹 내부 접근
			dialogBox = dialogInstance.transform.GetChild(0).gameObject;
			dialogText = dialogBox.GetComponentInChildren<TMP_Text>();
		}
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
