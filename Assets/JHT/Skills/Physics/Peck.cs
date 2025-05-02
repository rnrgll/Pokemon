using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Peck : SkillPhysic
{
	public Peck() : base("쪼기", "주둥이나 부리로 쪼아댄다",
		35, false, SkillType.Physical) { }
}
