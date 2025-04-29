using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
	public string Name { get; private set; }
	public int Level { get; private set; }
	public int HP { get; set; }
	public int MaxHP { get; private set; }
	public int Speed { get; private set; }
	public List<Skill> Skills { get; private set; }

	public Pokemon(string name, int maxHp, int speed, List<Skill> skills)
	{
		Name = name;
		MaxHP = maxHp;
		HP = maxHp;
		Speed = speed;
		Skills = skills;
	}
}