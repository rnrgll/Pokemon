using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Sleep : SkillStatus
{
    public Sleep() : base("잠자기 ", "바로 숙면을 취해 아픈 곳 없이 말끔히 낫는다. 한동안 푹 자다가 스스로 깬다",
		0, true, SkillType.Status,PokeType.Psychic,10,100) { }
}
