using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MakeBreeze : SkillPhysic
{
    public MakeBreeze() : base("바람일으키기", "세찬 바람을 일으켜 적을 타격한다",
		40, false, SkillType.Physical) { }
}
