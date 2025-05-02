using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TailWagging : SkillStatus
{
    public TailWagging() : base("꼬리흔들기", "귀엽게 꼬리를 흔들어 적이 경계를 게을리 하도록 만든다",
		0, false, SkillType.Status,PokeType.Normal,30,100) { }
}
