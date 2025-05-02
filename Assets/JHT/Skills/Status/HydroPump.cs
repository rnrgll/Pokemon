using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class HydroPump : SkillStatus
{
    public HydroPump() : base("겁나는얼굴", "무서운 얼굴로 위협한다. 상대는 움츠러들어 현저히 굼떠진다", 
		0, false, SkillType.Status) { }
}
