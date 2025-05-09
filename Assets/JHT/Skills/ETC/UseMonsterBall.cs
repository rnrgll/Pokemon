using UnityEngine;


public class UseMonsterBall : SkillS
{
	public UseMonsterBall() : base(
		"몬스터볼사용",
		"플레이어가 몬스터 볼을 사용해서 포켓몬을 잡는다.",
		0,
		Define.SkillType.ETC,
		true,
		Define.PokeType.None,
		0,
		100
	) { }



	public override void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		
	}
}
