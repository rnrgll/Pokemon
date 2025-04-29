using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JHTTEstEXP1 : MonoBehaviour
{
	//JHTTestExp exp;
	JHTTestHealth health;
	GameObject target;

    void Start()
    {
		//exp = FindObjectOfType<JHTTestExp>();
		health = FindObjectOfType<JHTTestHealth>();
	}

	public void ClickDamage()
	{
		if (health == null)
		{
			health = FindObjectOfType<JHTTestHealth>();
			
		}
		health.TakeDamage(5);
	}

	//public void Up()
	//{
	//	if (exp != null)
	//	{
	//		exp.GetXP(5);
	//	}
	//	else
	//	{
	//		Debug.LogError("exp가 null, GetXP를 호출안됨");
	//	}
	//}
}
