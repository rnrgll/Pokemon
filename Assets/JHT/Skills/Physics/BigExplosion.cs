using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BigExplosion : SkillPhysic
{
    public BigExplosion() : base("대폭발", "큰 폭발을 일으켜 주변에 피해를 주고 자신은 전투불능이 된다",
		250, false, SkillType.Physical,PokeType.Normal,5,100)
	{ }
}
