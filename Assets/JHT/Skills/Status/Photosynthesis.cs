using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Photosynthesis : SkillStatus
{
    public Photosynthesis() : base("광합성", "광합성을 하여 생산된 에너지로 활력을 되찾는다. 더 강한 햇빛을 받을수록 효율이 높아진다",
		0, true, SkillType.Status,PokeType.Grass,5,100) { }
}
