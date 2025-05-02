using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Winding : SkillPhysic
{
	public Winding() : base("휘감기", "넝쿨이나 촉수 등으로 휘감아 미미한 피해를 준다. 때때로 상대의 스피드를 떨어트린다",
		10, false, SkillType.Physical, PokeType.Normal, 25, 100)
	{ }
}
