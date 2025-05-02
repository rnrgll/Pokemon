using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog dialog;
    public void Interact(Vector2 position)
    {
	    Manager.Dialog.StartDialogue(dialog);
    }
}
