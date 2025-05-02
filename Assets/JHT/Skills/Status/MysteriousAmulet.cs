using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MysteriousAmulet : SkillStatus
{
    public MysteriousAmulet() : base("신비의부적", "신비한 힘이 상대로부터 오는 각종 방해로부터 아군 전체를 지켜준다",
		0, true, SkillType.Status,PokeType.Normal,25,100) { }
}
