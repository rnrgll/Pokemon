using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldObject : MonoBehaviour, IInteractable
{
	[SerializeField] string text;

	public void Interact(Vector2 position)
	{
		// TODO : 필드 오브젝트 상호작용 UI 활성화
		Manager.Data.PlayerData.Inventory.AddItem(text, 1);
		Debug.Log($"{gameObject.name} : {text}");
		gameObject.SetActive(false);
		Destroy(gameObject);
	}
}
