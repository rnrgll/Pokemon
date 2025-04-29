using UnityEngine;

// 배틀에서 실행될 행동(Attack 등)을 묶는 데이터 클래스
public class BattleAction
{
    public Pokemon Attacker { get; }
    public Pokemon Target { get; }
    public Skill Skill { get; }
   
    public BattleAction(Pokemon attacker, Pokemon target, Skill skill)
    {
        Attacker = attacker;
        Target = target;
        Skill = skill;
    }
}