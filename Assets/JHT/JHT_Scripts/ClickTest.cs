using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTest : MonoBehaviour
{
	PokeController controller;

	private void Start()
	{
		controller = FindObjectOfType<PokeController>();
	}

	public void Click()
	{
		if (controller == null) controller = FindObjectOfType<PokeController>();
		if (controller != null)
		{
			controller.Attack();
		}
		else
		{
			Debug.Log("타겟이 없습니다");
		}
	}
}
