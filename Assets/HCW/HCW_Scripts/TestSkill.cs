using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
	public string Name { get; private set; }
	public int Power { get; private set; }

	public Skill(string name, int power)
	{
		Name = name;
		Power = power;
	}
}
