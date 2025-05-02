using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class VineWhip : SkillSpecial
{
	public VineWhip() : base("덩굴채찍", "가는 덩굴로 채찍질한다",
		35, false, SkillType.Special,PokeType.Grass,10,100) { }
}
