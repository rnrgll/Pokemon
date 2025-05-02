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

		int healed = target.Heal(healAmount);
		inGameContext.NotifyMessage?.Invoke($"{target.pokeName}의 체력이 {healed} 회복되었다!");
		return true;
	}
}