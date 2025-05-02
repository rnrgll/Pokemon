using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Oblivion : SkillStatus
{
    public Oblivion() : base("망각술", "잠시 머리 속을 비워 잡념을 없애 특수방어가 부쩍 상승한다",
		0, true, SkillType.Status) { }
}
