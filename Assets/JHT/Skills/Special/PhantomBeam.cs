using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PhantomBeam : SkillSpecial
{
    public PhantomBeam() : base("환상빔", "이상야릇한 광선을 발사한다. 떄때로 약한 정신 분열을 유발한다",
		64, false, SkillType.Special) { }
}
