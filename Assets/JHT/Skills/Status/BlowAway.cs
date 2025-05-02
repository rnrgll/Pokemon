using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BlowAway : SkillStatus
{
    public BlowAway() : base("날려버리기", "상대 포켓몬을 멀리 날려버린다",
		0, false, SkillType.Status,PokeType.Normal,20,100) { }
}
