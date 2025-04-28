using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldObject : MonoBehaviour, IInteractable
{
	[SerializeField] string text;

	public void Interact()
	{
		Debug.Log($"{gameObject.name} : {text}");	
	}
}
