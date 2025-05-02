using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TestSkillExample
{
	public string Name, Desc;
	public int Damage, Accu, CurPP, MaxPP;
	public Define.SkillType SkillType;

	public abstract void Effect(Pokémon attacker, Pokémon defender);
}
