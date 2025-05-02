using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class DisgustingSound : SkillStatus
{
    public DisgustingSound() : base("싫은소리", "귀 따가운 소리를 내어 방어를 크게 떨어트린다",
		0, false, SkillType.Status) { }
}
