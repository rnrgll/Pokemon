using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CriticalFrontTeeth : SkillPhysic
{
    public CriticalFrontTeeth() : base("필살앞니", "날카로운 앞니로 콱 물어 본때를 보여 떄때로 상대를 풀이 죽게 한다",
		80, false, SkillType.Physical,PokeType.Normal,15,89.45f) { }
}
