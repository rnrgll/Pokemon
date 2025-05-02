using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(menuName = "Item/Composable")]
public class Item_Composable : ItemBase
{
	[SerializeField] private List<ScriptableObject> effectObjects;

	private List<IItemEffect> _effects;

	private void OnEnable()
	{
		_effects = new();
		
		if (effectObjects == null)
			return;

		foreach (var obj in effectObjects)
		{
			if (obj is IItemEffect effect)
				_effects.Add(effect);
		}
	}

	public override bool Use(Pokémon target, InGameContext context)
	{
		bool used = false;
		foreach (var effect in _effects)
		{
			used |= effect.Apply(target, context);
		}
		return used;
	}
}
