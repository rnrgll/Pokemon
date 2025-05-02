using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EatingDream : SkillSpecial
{
    public EatingDream() : base("꿈먹기", "자고 있는 포켓몬의 꿈을 먹어 생명력을 빼앗는다",
	100, false, SkillType.Special,PokeType.Psychic,15,100)
	{ }
}
