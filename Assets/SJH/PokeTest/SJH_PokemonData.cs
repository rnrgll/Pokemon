using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SJH_PokemonData
{

	private Dictionary<int, SJH_PokemonData> dataById;
	private Dictionary<string, SJH_PokemonData> dataByName;

	// 도감번호
	public int Id;
	// 이름
	public string Name;
	// 종족값
	public PokemonStat BaseStat;
	// 타입1
	public PokeType PokeType1;
	// 타입2
	public PokeType PokeType2;
	// 기술
	// 경험치 타입
	// 스프라이트1
	// 스프라이트2


	public void Init()
	{
		dataById = new Dictionary<int, SJH_PokemonData>()
		{
			[1] = new SJH_PokemonData
			{
				Id = 1,
				Name = "치코리타",
				BaseStat = new PokemonStat(45, 49, 65, 49, 65, 45),
				PokeType1 = PokeType.Grass,
				PokeType2 = PokeType.Poison,
			},
		};

		dataByName = new Dictionary<string, SJH_PokemonData>();
		foreach (var data in dataById.Values)
		{
			dataByName[data.Name] = data;
		};
	}

	public SJH_PokemonData GetPokemonData(int index)
	{
		var check = dataById[index];	
		if (check == null)
			return null;
		else
			return check;
	}

	public SJH_PokemonData GetPokemonData(string name)
	{
		var check = dataByName[name];
		if (check == null)
			return null;
		else
			return check;
	}
}
