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
    public static DialogManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public void ShowText(Dialog dialog)
    {
        dialogBox.SetActive(true);
        dialogText.text = dialog.Lines[0];
    }

    public IEnumerator ShowDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.5f / letterPerSec);
        }
        yield return new WaitForSeconds(0.5f);
    }

}
