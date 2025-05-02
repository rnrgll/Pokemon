using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ComradeGil : SkillStatus
{
    public ComradeGil() : base("길동무", "자신을 쓰러트린 적 포켓몬을 길동무 삼아 같이 기절한다",
		0, false, SkillType.Status,PokeType.Ghost,5,100) { }
}
