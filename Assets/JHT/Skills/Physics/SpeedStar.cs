using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SpeedStar : SkillPhysic
{
	public SpeedStar() : base("스피드스타", "빗나가지 않는 별 모양의 빛을 날린다",
		60, false, SkillType.Physical) { }
}
