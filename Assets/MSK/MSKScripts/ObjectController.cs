using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog dialog;
    public void Interact()
    {
	    Manager.Dialog.StartDialogue(dialog);
    }
}
