using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CurtainOfLight : SkillStatus
{
    public CurtainOfLight() : base("빛의장막", "불가사의한 장막을 형성해 특수 공격으로 입는 피해를 줄인다"
		, 0, true, SkillType.Status,PokeType.Psychic,30,100) { }
}
