using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillStatus : SkillS
{
	public SkillStatus(string name, string description, float damage, bool isMyStat, SkillType skillType) :
		base(name, description, damage, isMyStat, skillType)
	{ }

	public override void UseSkill(PokemonS attacker, PokemonS defender, SkillS skill)
	{
	
	}
}
