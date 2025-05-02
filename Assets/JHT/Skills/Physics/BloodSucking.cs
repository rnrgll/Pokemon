using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BloodSucking : SkillPhysic
{
    public BloodSucking() : base("흡혈", "이빨로 깨물고 피를 빨아 생명력을 빼앗는다",
		20, false, SkillType.Physical,PokeType.Bug,15,100) { }
}
