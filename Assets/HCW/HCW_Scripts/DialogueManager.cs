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

	public void Awake()
	{
		dialoguePanel.setActive(false);
	}
}
