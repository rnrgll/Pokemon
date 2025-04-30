using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	[SerializeField] private GameObject dialoguePanel;
	[SerializeField] private TMP_Text dialogueText;

	private Queue<string> sentences = new Queue<string>();

	void Awake()
	{
		// 씬 로드 시 대사창은 숨겨둠
		dialoguePanel.SetActive(false);
	}

	// 외부에서 대사 리스트를 넘겨받아 대사창을 열고
	// 첫 번째 문장을 즉시 표시하기 위한 메서드
	public void StartDialogue(IEnumerable<string> lines)
	{
		// 이전에 남은 문장이 있으면 모두 삭제
		sentences.Clear();
		// 매개변수로 받은 대사들을 순서대로 큐에 추가
		foreach (var line in lines)
			sentences.Enqueue(line);
		// 대사창 보이기
		dialoguePanel.SetActive(true);
	}
}
