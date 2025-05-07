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

	public Define.NpcState npcState = Define.NpcState.Idle;


	Dialog dialog;

	// NPC대사 단위
	int currentLine = 0;
	//	대사 출력의 여부
	public bool isTyping;
	
	private bool haveToPreventInput = false;

	public static DialogManager Instance { get; private set; }


	public event Action OnShowDialog;
	public event Action CloseDialog;

	protected override void Awake()
	{
		Instance = this;
	}

	public void HandleUpdate()
	{   // 대화 진행중
		if (haveToPreventInput) return;
		
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
					//Manager.Game.Player.State = Define.PlayerState.Field;
					Manager.Game.Player.State = Manager.Game.Player.prevState;
					Manager.Dialog.npcState = Define.NpcState.Idle;
					CloseDialog?.Invoke();
				}
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
	public void StartDialogue(Dialog dialog)
	{
		Manager.Game.Player.State = Define.PlayerState.Dialog;
		CreateDialogueUI();
		StartCoroutine(DialogManager.Instance.ShowText(dialog));
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
	
	
	
	public IEnumerator ShowBattleMessage(string message)
	{
		Manager.Game.Player.State = Define.PlayerState.Dialog;
		CreateDialogueUI();

		haveToPreventInput = true;
		dialogBox.SetActive(true);
		dialogText.text = "";

		foreach (var letter in message.ToCharArray())
		{
			dialogText.text += letter;
			yield return new WaitForSeconds(0.6f / letterPerSec);
		}

		yield return new WaitForSeconds(1f); // 읽는 시간 확보
		dialogBox.SetActive(false);

		haveToPreventInput = false;
		Manager.Game.Player.State = Define.PlayerState.Field;
	}

}
