using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PokeType
{
	None,
	Fire,
	Water,
	Wind,
	Grass,
	Ice,
	Fighting,
	Poison,
	Ground
}

public class PokeClass : MonoBehaviour
{
	public int id;
	public bool isDead;
	public PokeType type;
	public Sprite icon;
	private int maxHp = 20;
	public int curHp;
	//public int Hp { get; set; }
	public float exp;
	//public float Exp { get { return exp; } }
	public float dieExp;
	public int power;
	//public int Power { get; set; }

	public void Start()
	{
		curHp = maxHp;
	}

}
