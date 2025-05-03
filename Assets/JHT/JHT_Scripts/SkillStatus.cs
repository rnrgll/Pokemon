using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillStatus : SkillS
{
	public SkillStatus(string name, string description, float damage, bool isMyStat, SkillType skillType, PokeType type, int pp, float accuracy, bool isHm = false) :
		base(name, description, damage, skillType, isMyStat, type,pp,accuracy,isHm)
	{ }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
	
	}
}
