using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SmokeScreen : SkillStatus
{
    public SmokeScreen() : base("연막", "뿌연 연기나 진한 먹물을 뿌려 상대의 시야를 흐려 명중률을 떨어트린다",
		0, false, SkillType.Status) { }
}
