using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryTreeObject : MonoBehaviour, IInteractable
{
	public void Interact()
	{
		Debug.Log("나무열매얻기");
	}
}
