using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour, IInteractable
{
	[SerializeField] Animator anim;
	[SerializeField] Dialog firstDialog;
	[SerializeField] Dialog afterHealDialog;

	[SerializeField] GameObject[] balls;
	[SerializeField] Animator[] ballAnims;

	Coroutine healerCoroutine;

	void Start()
	{
		anim = GetComponent<Animator>();
		foreach (GameObject go in balls)
		{
			go.SetActive(false);
		}

		for (int i = 0; i < balls.Length; i++)
		{
			ballAnims[i] = balls[i].GetComponent<Animator>();
			balls[i].SetActive(false);
		}
	}

	public void Interact(Vector2 position)
	{
		if (!Manager.Dialog.isTyping)
		{
			if (healerCoroutine == null)
				healerCoroutine = StartCoroutine(Heal());
		}
	}

	private IEnumerator Heal()
	{
		Manager.Dialog.npcState = Define.NpcState.Talking;

		string currentSceneName = Manager.Game.Player.CurSceneName;

		SoundManager sound = Manager.Game.Player.gameObject.GetComponentInChildren<SoundManager>();


		// 첫 대사
		Debug.Log("첫대사 시작");
		Manager.Dialog.StartDialogue(firstDialog);
		// 대사 끝날때까지 대기
		yield return UntilDialogClose();

		// 대사
		yield return new WaitForSeconds(1f);

		// 스프라이트 변경
		anim.SetTrigger("heal");
		for (int i = 0; i < Manager.Poke.party.Count; i++)
		{
			balls[i].SetActive(true);
			yield return new WaitForSeconds(0.5f);
		}
		for (int i = 0; i < Manager.Poke.party.Count; i++)
		{
			ballAnims[i].SetTrigger("heal");
		}

		// 브금 실행
		//Manager.Game.Player.CurSceneName = "Heal";
		sound.Play("Heal");
		sound.GetComponent<AudioSource>().loop = false;

		// 포켓몬 회복
		if (Manager.Poke.PartyHeal())
			Debug.Log("포켓몬 회복 완료");

		yield return new WaitForSeconds(Manager.Poke.party.Count * 0.5f);

		anim.SetTrigger("end");
		for (int i = 0; i < Manager.Poke.party.Count; i++)
		{
			balls[i].SetActive(false);
			ballAnims[i].SetTrigger("default");
		}

		// 다음 대사
		Debug.Log("마지막대사 시작");
		Manager.Dialog.StartDialogue(afterHealDialog);
		// 대사끝날때까지 대기
		yield return UntilDialogClose();

		anim.SetTrigger("default");

		// bgm 다시 재생
		Manager.Game.Player.CurSceneName = currentSceneName;
		sound.GetComponent<AudioSource>().loop = true;

		Manager.Dialog.npcState = Define.NpcState.Idle;
		healerCoroutine = null;
	}


	IEnumerator UntilDialogClose()
	{
		bool isDone = false;

		void OnClose() => isDone = true;

		// 이벤트 등록
		Manager.Dialog.CloseDialog += OnClose;
		// 대사 끝날때까지 대기
		yield return new WaitUntil(() => isDone);
		// 이벤트 삭제
		Manager.Dialog.CloseDialog -= OnClose;
	}

}
