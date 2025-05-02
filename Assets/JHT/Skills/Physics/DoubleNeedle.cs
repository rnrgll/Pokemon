using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class DoubleNeedle : SkillPhysic
{
    public DoubleNeedle() :base("더블니들", "2개의 뾰족한 바늘로 연달아 찌른다. 때때로 상대를 중독시킨다",
		25, false, SkillType.Physical) { }
}
