using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Recharge : SkillStatus
{
    public Recharge() : base("기충전", "깊게 숨을 들이쉬고 기합을 넣어 집중력을 높힌다",
		0, true, SkillType.Status) { }
}
