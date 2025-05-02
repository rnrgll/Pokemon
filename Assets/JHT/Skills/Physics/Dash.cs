using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Dash : SkillPhysic
{
    public Dash() : base("돌진", "자신도 다칠 수 있지만 앞뒤를 가리지 않고 돌진해 들이 받는다",
		90, false, SkillType.Physical,PokeType.Normal,20,84.38f) { }
}
