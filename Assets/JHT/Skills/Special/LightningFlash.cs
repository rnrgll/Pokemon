using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class LightningFlash : SkillSpecial
{
    public LightningFlash() :base("전광석화", "눈으로 쫓을 수 없을 정도의 엄청난 속도로 돌진한다",
		40, false, SkillType.Special,PokeType.Normal,30,100) { }
}
