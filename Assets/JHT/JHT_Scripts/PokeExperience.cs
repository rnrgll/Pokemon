using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeExperience : MonoBehaviour
{
	PokeController controller;

	

	void Start()
	{
		controller = GetComponent<PokeController>();
	}

	public void GetXp(float amount)
	{
		if (controller.gameObject.layer==10)
		{
			controller.exp += amount;
		}
		
		//if(exp가 GameManager.Instance.경험치최대치)
		//{
		//	진화이벤트;
		//}
	}
}
