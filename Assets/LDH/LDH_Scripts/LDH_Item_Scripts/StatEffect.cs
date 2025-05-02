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
		Debug.Log($"{statType.ToString()}을 사용합니다.");
		return true;
	}
}
