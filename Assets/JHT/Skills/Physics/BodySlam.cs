using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BodySlam : SkillPhysic
{
	public BodySlam() : base("몸통박치기", "몸전체를 이용해 들이받는다",
		35, false, SkillType.Physical,PokeType.Normal,35,94.53f) { }
	
}
