using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Roll : SkillPhysic
{
	int count =0;
	int curpp = 0;
    public Roll() : base("구르기", "스스로 걷잡을 수 없이 계속 구른다. 미리 웅크렸다면 더욱 거세진다",
		30, false, SkillType.Physical, PokeType.Rock,20,89.45f)
	{ }

	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		int rand = Random.Range(0, 100);
		defender.animator.SetTrigger(name);

		//랜덤변수
		if (Mathf.RoundToInt(accuracy) >= rand)
		{
			if (count > 0)
			{
				skill.curPP = curpp;
				if (count >= 5)
					count = 0;
			}
			defender.TakeDamage(attacker, defender, skill);
			skill.curPP--;
			curpp = skill.curPP;
			count++;
		}
		else
		{
			Debug.Log("공격을 회피하였습니다");
			count = 0;
		}
	}
}
