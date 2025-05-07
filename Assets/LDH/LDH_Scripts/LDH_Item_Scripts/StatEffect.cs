using UnityEditor.Experimental.GraphView;
using UnityEngine;


public enum StatType
{
	Attack,
	Defense,
	SpeAttack,
	SpeDefense,
	Speed
}
[CreateAssetMenu(menuName = "ItemEffect/BoostStat")]
public class StatEffect : ScriptableObject, IItemEffect
{
	[SerializeField] private StatType statType;
	
	[SerializeField] private int boostRankAmount;
	
	public bool Apply(Pokémon target, InGameContext inGameContext)
	{
		//Todo: 배틀 소모성 도구 아이템 기능 구현

		// 효과 적용
		switch (statType)
		{
			case StatType.Attack:
				target.pokemonBattleStack.attack = Mathf.Min(6, target.pokemonBattleStack.attack + boostRankAmount);
				break;
			case StatType.Defense:
				target.pokemonBattleStack.defense = Mathf.Min(6, target.pokemonBattleStack.defense + boostRankAmount);
				break;
			case StatType.SpeAttack:
				target.pokemonBattleStack.speAttack = Mathf.Min(6, target.pokemonBattleStack.speAttack + boostRankAmount);
				break;
			case StatType.SpeDefense:
				target.pokemonBattleStack.speDefense = Mathf.Min(6, target.pokemonBattleStack.speDefense + boostRankAmount);
				break;
			case StatType.Speed:
				target.pokemonBattleStack.speed = Mathf.Min(6, target.pokemonBattleStack.speed + boostRankAmount);
				break;
		}
		// TODO : 효과 적용 후 메뉴 닫혀야함
		Debug.Log($"{statType.ToString()}을 사용합니다.");
		return true;
	}
}
