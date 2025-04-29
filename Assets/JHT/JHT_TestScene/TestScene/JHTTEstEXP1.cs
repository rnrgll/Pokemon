using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JHTTEstEXP1 : MonoBehaviour
{
	JHTTestExp exp;
    void Start()
    {
        exp = FindObjectOfType<JHTTestExp>();
    }

    public void Up()
	{
		if (exp != null)
		{
			exp.GetXP(5);
		}
		else
		{
			Debug.LogError("exp가 null, GetXP를 호출안됨");
		}
	}
}
