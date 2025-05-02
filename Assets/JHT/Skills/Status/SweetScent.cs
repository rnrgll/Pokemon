using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SweetScent : SkillStatus
{
    public SweetScent() : base("달콤한향기", "달콤한 향기를 풍겨 포켓몬이 몰려들게 한다. 향기를 맡은 적은 정신이 팔려 반사신경이 저하된다",
		0, false, SkillType.Status) { }
}
