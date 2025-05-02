using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Curse : SkillStatus
{
	public Curse() : base("저주", "자신의 무언가를 깎아내려 포켓몬에게 변화를 일으킨다",
		0, false, SkillType.Status,PokeType.None,10,100) { } //둘다 작용해야됨
}
