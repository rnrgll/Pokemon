using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulbasaur : PokemonS
{
	public Bulbasaur(int level) : base
	(
		_id: 10,
		_name: "이상해씨",
		_level: level,
		_baseStat: PokemonManagerS.Get.GetBaseStat[10],
		_iv: PokemonIVS.GetRandomIV(),
		_pokeType1: PokeType.Grass,
		_pokeType2: PokeType.Ground
	)
	{

	}
}
