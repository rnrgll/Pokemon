using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Crouch : SkillStatus
{
    public Crouch() : base("웅크리기", "공격에 대비해 몸을 웅크려 둥글게 한다", 
		0, true, SkillType.Status,PokeType.Normal,40,100) { }
}
