using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Mesmerism : SkillStatus
{
    public Mesmerism() :base("최면술", "최면을 걸어 잠들게 한다",
		0, false, SkillType.Status) { }
}
