using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class HighSpeedMovement : SkillStatus
{
    public HighSpeedMovement() : base("고속이동", "힘을 빼고 몸을 가볍게 해 매우 빨리 움직일 수 있게 된다",
		0, true, SkillType.Status,PokeType.Psychic,30,100) { }
}
