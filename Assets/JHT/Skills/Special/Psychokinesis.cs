using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Psychokinesis : SkillSpecial
{
    public Psychokinesis() : base("사이코키네시스", "강한 염력을 보내 공격한다. 때때로 상대의 특수방어를 떨어트린다",
		90, false, SkillType.Special,PokeType.Psychic,10,90) { }
}
