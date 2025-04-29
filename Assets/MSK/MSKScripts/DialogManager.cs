using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] TMP_Text dialogText;
    [SerializeField] int letterPerSec;


    public event Action OnShowDialog;
    public event Action CloseDialog;


    Dialog dialog;
    int  currentLine = 0;
    bool isTyping;
    public static DialogManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
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

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isTyping) {
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(ShowDialog(dialog.Lines[currentLine]));
            }
            else {
                currentLine = 0;
                dialogBox.SetActive(false);
                CloseDialog?.Invoke();

            }
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
