using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Reflector : SkillStatus
{
   public Reflector() : base("리플렉터", "불가사의한 장벽을 형성해 물리 공격으로 입는 피해를 줄인다",
	   0, true, SkillType.Status) { }
}
