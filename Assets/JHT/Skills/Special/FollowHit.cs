using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FollowHit : SkillSpecial
{
    public FollowHit() : base("따라가때리기", "교체하려는 낌새를 보이면, 교체하기 전에 강하게 후려친다",
		40, false, SkillType.Special,PokeType.Dark,20,100) { }
}
