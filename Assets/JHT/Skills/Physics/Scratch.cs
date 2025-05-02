using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Scratch : SkillPhysic
{
	public Scratch() : base("할퀴기", "단단하고, 뾰족하면서 날카로운 손톱이나 발톱으로 상대를 할퀸다.", 
		40, false, SkillType.Physical,PokeType.Normal,35,100) { }
	
}
