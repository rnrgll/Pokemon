using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryTreeObject : MonoBehaviour, IInteractable
{
	public void Interact(Vector2 position)
	{
		Debug.Log("나무열매얻기");
	}
}
