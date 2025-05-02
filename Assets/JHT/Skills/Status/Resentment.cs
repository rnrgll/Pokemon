using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Resentment : SkillStatus
{
    public Resentment() : base("원한", "상대의 마지막 기술에 앙심을 품어 기술을 선보일 기회를 줄여버린다",
		0, false, SkillType.Status,PokeType.Ghost,10,100) { }
}
