using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Telekinesis : SkillSpecial
{
    public Telekinesis() : base("염동력", "약한 염력을 보내 공격한다. 상대는 때때로 정신이 혼미해진다",
		50, false, SkillType.Special,PokeType.Psychic,25,100) { }
}
