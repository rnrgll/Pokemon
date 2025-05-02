using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SeeThrough : SkillStatus
{
    public SeeThrough() : base("꿰뚫어보기", "상대를 면밀히 파악한다. 간파된 포켓몬에게는 이후의 기술이 잘 맞게 된다", 
		0, false, SkillType.Status,PokeType.Normal,40,100) { }
}
