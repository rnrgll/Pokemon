using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class LeafCutting : SkillSpecial
{
    public LeafCutting() : base("잎날가르기", "예리한 잎들을 날려 적을 베어버린다. 급소에 맞히기 쉽다", 
		55, false, SkillType.Special,PokeType.Grass,25,95.53f) { }
}
