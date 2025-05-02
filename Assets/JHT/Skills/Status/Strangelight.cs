using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Strangelight : SkillStatus
{
    public Strangelight() : base("이상한빛", "쪼이면 발작을 일으키는 괴상한 빛을 발산한다",
		0, false, SkillType.Status) { }
}
