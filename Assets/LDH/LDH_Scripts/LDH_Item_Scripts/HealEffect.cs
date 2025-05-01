using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/Heal")]
public class HealEffect : ScriptableObject, IItemEffect
{
	[SerializeField] private int healAmount;

	public bool Apply(Pokémon target, InGameContext context)
	{
		if (target.hp >= target.maxHp)
		{
			context.NotifyMessage?.Invoke("효과가 없다!");
			return false;
		}

		int healed = target.Heal(healAmount);
		context.NotifyMessage?.Invoke($"{target.pokeName}의 체력이 {healed} 회복되었다!");
		return true;
	}
}