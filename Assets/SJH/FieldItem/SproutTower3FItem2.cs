using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SproutTower3FItem2 : MonoBehaviour, IInteractable
{
	public static bool isGet;
	[SerializeField] string itemName;

	void Start()
	{
		if (!isGet)
			gameObject.SetActive(true);
		else
			gameObject.SetActive(false);
	}

	public void Interact(Vector2 position)
	{
		if (!isGet)
		{
			isGet = true;
			Manager.Data.PlayerData.Inventory.AddItem(itemName, 1);
			Debug.Log($"{gameObject.name} : {itemName}");
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}
}
