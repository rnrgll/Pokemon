using UnityEngine;

public interface IItemEffect
{
	bool Apply(Pokémon target, InGameContext inGameContext);
}
