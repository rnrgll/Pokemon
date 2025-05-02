using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class WingBeat : SkillPhysic
{
    public WingBeat() : base("날개치기", "날개를 크고 넓게 펼쳐서 가격한다",
		60, false, SkillType.Physical) { }
}
