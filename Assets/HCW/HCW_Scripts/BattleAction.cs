using UnityEngine;

// 배틀에서 실행될 행동(Attack 등)을 묶는 데이터 클래스
public class BattleAction
{
    public Pokémon Attacker { get; }
    public Pokémon Target { get; }
    public string Skill { get; }
   
    public BattleAction(Pokémon attacker, Pokémon target, string skill)
    {
        Attacker = attacker;
        Target = target;
        Skill = skill;
    }
}