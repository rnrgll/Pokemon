using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeController : MonoBehaviour
{
	public bool isMine;
	public Sprite icon;
	private int maxHp = 20;
	public int curHp;
	//public int Hp { get; set; }

	public float exp;
	//public float Exp { get { return exp; } }
	public float dieXp;

	public int power;
	//public int Power { get; set; }

	PokeHealth target;

	public void Start()
	{
		curHp = maxHp;
	}

	public void Attack()
	{
		target.GetComponent<PokeHealth>();
		if (target.gameObject.layer == 11)
		{
			target.TakeDamage(power);
		}
	}


}
