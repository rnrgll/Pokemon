using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Ultrasound : SkillStatus
{
    public Ultrasound() : base("초음파", "몸에서 발산하는 초음파로 상대를 혼란시킨다",
		0, false, SkillType.Status) { }
}
