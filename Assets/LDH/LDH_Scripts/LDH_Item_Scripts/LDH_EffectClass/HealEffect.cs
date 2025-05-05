using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/Heal")]
public class HealEffect : ScriptableObject, IItemEffect
{
	[SerializeField] private int healAmount;

	public bool Apply(Pokémon target, InGameContext inGameContext)
	{
		if (target.hp >= target.maxHp)
		{
			inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.NoEffect));
			return false;
		}

		int oldHp = target.hp; // Heal 호출 전에 저장해둬야 함
		int healed = target.Heal(healAmount); // hp 변화 발생
		int newHp = target.hp;
		
		if (inGameContext.PokemonSlot != null)
		{
			inGameContext.PokemonSlot.StartSliderAnimation(oldHp,newHp,target.maxHp);
		}
		inGameContext.NotifyMessage?.Invoke($"{target.pokeName}의 체력이 {healed} 회복되었다!");
		return true;
	}
}