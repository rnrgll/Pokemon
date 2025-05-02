using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Growth : SkillStatus
{
    public Growth() : base("성장", "몸을 단숨에 성장시켜 특수공격을 높힌다",
		0, true, SkillType.Status) { }
}
