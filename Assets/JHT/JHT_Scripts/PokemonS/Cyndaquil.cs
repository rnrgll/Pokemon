using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyndaquil : PokemonS
{
	public Cyndaquil(int level) : base
	(
		_id: 3,
		_name: "브케인",
		_level: level,
		_baseStat: PokemonManagerS.Get.GetBaseStat[3],
		_iv: PokemonIVS.GetRandomIV(),
		_pokeType1: PokeType.Fire,
		_pokeType2: PokeType.Ground
	)
	{

	}
}
