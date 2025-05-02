using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Roll : SkillPhysic
{
    public Roll() : base("구르기", "스스로 걷잡을 수 없이 계속 구른다. 미리 웅크렸다면 더욱 거세진다",
		30, false, SkillType.Physical, PokeType.Rock,20,89.45f)
	{ }
}
