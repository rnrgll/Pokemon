using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Bite : SkillSpecial
{
    public Bite() : base("물기", "날카로운 송곳니로 포악하게 깨문다. 때때로 상대를 풀이 죽게 만든다",
		60, false, SkillType.Special,PokeType.Dark,25,100) { }
}
