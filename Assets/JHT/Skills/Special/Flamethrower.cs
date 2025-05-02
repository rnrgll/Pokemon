using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Flamethrower : SkillSpecial
{
    public Flamethrower() : base("화염방사", "강렬한 불줄기를 뿜는다 상대에게 때때로 화상을 입힌다",
		95, false, SkillType.Special,PokeType.Fire,15,100) { }
}
