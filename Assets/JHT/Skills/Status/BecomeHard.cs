using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BecomeHard : SkillStatus
{
    public BecomeHard() : base("단단해지기", "몸 전체에 힘을 줘 단단하게 한다", 
		0, true, SkillType.Status,PokeType.Normal,30,100) { }
}
