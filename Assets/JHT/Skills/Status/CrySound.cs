using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CrySound : SkillStatus
{
    public CrySound() : base("울음소리", "애교 석인 울음 소리를 내어 적들이 살살 공격하게 만든다",
		0, false, SkillType.Status,PokeType.Normal,40,100) { }
}
