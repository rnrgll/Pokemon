using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SprinkleSand : SkillStatus
{
    public SprinkleSand() : base("모래뿌리기", "상대의 얼굴에 모래를 확 뿌린다. 모래 때문에 잘 볼 수 없어 기술이 빗나가기 쉬워진다",
		0, false, SkillType.Status,PokeType.Ground,15,100) { }
}
