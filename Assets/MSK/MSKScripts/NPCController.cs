using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog dialog;
    public void Interact()
    {
        Debug.Log("NPC Interact success");
		if (DialogManager.Instance.isTyping == false) { 
        StartCoroutine(DialogManager.Instance.ShowText(dialog));
		}
	}

}
