using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SolarBeam : SkillSpecial
{
    public SolarBeam() : base("솔라빔", "잠시동안 햇빛을 모은 후, 태양광선을 발사한다",
		120, false, SkillType.Special) { }
}
